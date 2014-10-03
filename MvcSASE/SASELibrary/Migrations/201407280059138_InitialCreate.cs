using System.Data.Entity.Migrations;

namespace SASELibrary.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountServices",
                c => new
                {
                    ID = c.Int(false, true),
                    storageAccount = c.String(),
                    storageKey = c.String(),
                    userEmail = c.String(),
                    Discriminator = c.String(false, 128),
                })
                .PrimaryKey(t => t.ID);
        }

        public override void Down()
        {
            DropTable("dbo.AccountServices");
        }
    }
}