using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MvcSASE.Models
{
    public class SASE
    {
        public int ID { get; set; }
        public string userEmail { get; set; }
        public string storageAccount { get; set; }
        public string storageKey { get; set; }
    }

    public class SASEDBContext : DbContext
    {
        public DbSet<SASE> Sase { get; set; }
    }
}