namespace ContactManager.Migrations
{
	using System.Data.Entity.Migrations;

	internal sealed class Configuration : DbMigrationsConfiguration<ContactManager.ContactManagerContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
		}

		protected override void Seed(ContactManager.ContactManagerContext context)
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.

			context.Contacts.AddOrUpdate(
			  new Kontakt { Vorname = "Manuel", Nachname = "Naujoks" },
			  new Kontakt { Vorname = "Max", Nachname = "Musterman" }
			);

		}
	}
}
