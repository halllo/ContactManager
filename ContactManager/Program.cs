using JustObjectsPrototype;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Windows;

namespace ContactManager
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			ObservableCollection<object> repository;


			using (var db = new ContactManagerContext())
			{
				var contacts = (from c in db.Contacts
								orderby c.Nachname, c.Vorname
								select c).ToList();

				repository = new ObservableCollection<object>(contacts);
			}


			Show.Prototype(With.Objects(repository)
				.AndViewOf<Kontakt>().EnableDelete(deleted =>
				{
					if (deleted.Id != 0)
					{
						using (var db = new ContactManagerContext())
						{
							db.Contacts.Attach(deleted);
							db.Contacts.Remove(deleted);
							db.DeleteLogs.Add(new DeleteLog
							{
								DeleteDate = DateTime.Now,
								Metadata = deleted.Vorname + " " + deleted.Nachname
							});
							db.SaveChanges();
						}
					}
				})
				.EnableNew(newed => newed.Geändert = true)
				.OnValueChanged(changed => changed.Geändert = true)
			);
		}
	}





	public class ContactManagerContext : DbContext
	{
		public DbSet<Kontakt> Contacts { get; set; }
		public DbSet<DeleteLog> DeleteLogs { get; set; }
	}

	public class DeleteLog
	{
		public int Id { get; set; }
		public string Metadata { get; set; }
		public DateTime DeleteDate { get; set; }
	}

	public class Kontakt
	{
		[NotMapped]
		public bool Geändert { get; set; }
		public int Id { get; set; }
		public string Vorname { get; set; }
		public string Nachname { get; set; }


		public static void Speichern(ObservableCollection<Kontakt> contacts)
		{
			using (var db = new ContactManagerContext())
			{
				foreach (var contact in contacts.Where(c => c.Geändert))
				{
					contact.Geändert = false;

					if (contact.Id != 0)
					{
						db.Contacts.Attach(contact);
						var entry = db.Entry(contact);
						entry.Property(e => e.Vorname).IsModified = true;
						entry.Property(e => e.Nachname).IsModified = true;
					}
					else
					{
						db.Contacts.Add(contact);
					}
				}
				db.SaveChanges();
			}
		}

		public static void Löschprotokoll()
		{
			List<DeleteLog> deleteLogs;
			using (var db = new ContactManagerContext())
			{
				deleteLogs = db.DeleteLogs
					.OrderByDescending(l => l.DeleteDate)
					.Take(20)
					.ToList();
			}

			MessageBox.Show(string.Join("\n", deleteLogs
				.Select(l => l.DeleteDate.ToString("yyyy.MM.dd HH:mm:ss") + " " + l.Metadata)),
				"Letzte 20 Löschungen");
		}
	}
}
