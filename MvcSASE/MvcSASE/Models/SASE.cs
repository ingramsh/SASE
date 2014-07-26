using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using SASELibrary;
using System.ComponentModel.DataAnnotations;

namespace MvcSASE.Models
{
    public class SASE
    {
        [NotMapped]
        public int? passID { get; set; }
        [NotMapped]
        public string containerName { get; set; }
        [NotMapped]
        public int blobID { get; set; }
        [NotMapped]
        public List<string> blobInfo { get; set; }
        [NotMapped]
        public string queueName { get; set; }
        [NotMapped]
        bool active { get; set; }
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

    public class SASEUploadBlob
    {
        [Required]
        public string uploadFilePath { get; set; }
    }
}