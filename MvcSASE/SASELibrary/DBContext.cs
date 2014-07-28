using System.Data.Entity;

namespace SASELibrary
{
    public class DBContext : DbContext
    {
        public DBContext()
            : base("SASEDBContext") { }
        public DbSet<AzureAccountService> Sase { get; set; }
    }
}
