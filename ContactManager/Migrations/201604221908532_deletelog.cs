namespace ContactManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletelog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeleteLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Metadata = c.String(),
                        DeleteDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeleteLogs");
        }
    }
}
