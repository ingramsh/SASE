using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace SASELibrary
{
    public abstract class AccountService
    {
        [NotMapped]
        private bool active { get; set; }

        [NotMapped]
        public int blobID { get; set; }

        [NotMapped]
        public BlobInfo blobInfo { get; set; }

        [NotMapped]
        public string containerName { get; set; }

        public int ID { get; set; }

        [NotMapped]
        public int? passID { get; set; }

        [NotMapped]
        public string queueName { get; set; }

        public AccountService service
        {
            get { return this; }
        }

        public string storageAccount { get; set; }
        public string storageKey { get; set; }
        public string userEmail { get; set; }
        public abstract IEnumerable<string> BlobContainerNames();

        public abstract BlobInfo BlobInfo(string container, string item);
        public abstract IEnumerable<string> BlobItemNames(string container);
        public abstract IEnumerable<string> BlobItems(string container);

        public abstract bool CreateContainer(string name);
        public abstract bool CreateQueue(string name);
        public abstract string DequeueMessage(string name);
        public abstract byte[] DownloadBlobBytes(string container, string item);
        public abstract Stream DownloadBlobStream(string container, string item);
        public abstract bool EnqueueMessage(string name, string message);

        public abstract Message PeekMessage(string name);
        public abstract int QueueCount();
        public abstract int QueueMessageCount(string name);

        public abstract IEnumerable<string> QueueNames();

        public abstract bool UploadBlockBlobStream(string container, string name, Stream file);
    }
}