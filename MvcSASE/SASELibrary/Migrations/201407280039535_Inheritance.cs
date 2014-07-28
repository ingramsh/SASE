namespace SASELibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inheritance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountServices",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        storageAccount = c.String(),
                        storageKey = c.String(),
                        userEmail = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            DropTable("dbo.AccountServices");
        }
        
        public override void Down()
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
            
            DropTable("dbo.AccountServices");
        }
    }
}
