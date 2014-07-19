using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using SASELibrary;

namespace MvcSASE.Models
{
    public class SASE
    {
        [NotMapped]
        public SASEAccountService service
        {
            get
            {
                return new SASELibrary.SASEAccountService(this.storageAccount, this.storageKey);
            }
        }
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