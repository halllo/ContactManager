namespace ContactManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class contactrename : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Contacts", newName: "Kontakts");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Kontakts", newName: "Contacts");
        }
    }
}
