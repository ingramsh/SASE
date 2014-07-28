namespace SASELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountServices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userEmail = c.String(),
                        storageAccount = c.String(),
                        storageKey = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountServices");
        }
    }
}
