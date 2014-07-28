using System;
using System.ComponentModel.DataAnnotations.Schema;
namespace SASELibrary
{
    public abstract class AccountService
    {
        public abstract System.Collections.Generic.IEnumerable<string> BlobContainerNames();
        [NotMapped]
        bool active { get; set; }
        [NotMapped]
        public int blobID { get; set; }
        [NotMapped]
        public BlobInfo blobInfo { get; set; }
        public abstract BlobInfo BlobInfo(string container, string item);
        public abstract System.Collections.Generic.IEnumerable<string> BlobItemNames(string container);
        public abstract System.Collections.Generic.IEnumerable<string> BlobItems(string container);
        [NotMapped]
        public string containerName { get; set; }
        public abstract bool CreateContainer(string name);
        public abstract bool CreateQueue(string name);
        public abstract string DequeueMessage(string name);
        public abstract bool DownloadBlobBlock(string container, string item, string filepath);
        public abstract byte[] DownloadBlobBytes(string container, string item);
        public abstract System.IO.Stream DownloadBlobStream(string container, string item);
        public abstract bool EnqueueMessage(string name, string message);
        public int ID { get; set; }
        [NotMapped]
        public int? passID { get; set; }
        public abstract Message PeekMessage(string name);
        public abstract int QueueCount();
        public abstract int QueueMessageCount(string name);
        [NotMapped]
        public string queueName { get; set; }
        public abstract System.Collections.Generic.IEnumerable<string> QueueNames();
        public AccountService service
        {
            get
            {
                return this;
            }
        }
        public string storageAccount { get; set; }
        public string storageKey { get; set; }
        public abstract bool UploadBlockBlob(string container, string filepath);
        public abstract bool UploadBlockBlobBytes(string container, string name, byte[] file);
        public abstract bool UploadBlockBlobStream(string container, string name, System.IO.Stream file);
        public string userEmail { get; set; }
    }
}
