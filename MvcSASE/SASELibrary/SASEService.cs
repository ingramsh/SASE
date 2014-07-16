using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class SASEService
    {
        private StorageCredentials creds;
        private CloudStorageAccount account;
        private SASEBlob blob;
        private SASEQueue queue;

        public SASEService(string name, string key)
        {
            creds = new StorageCredentials(name, key);
            account = new CloudStorageAccount(creds, false);
            blob = new SASEBlob(account);
            queue = new SASEQueue(account);
        }

        public List<string> BlobContainerNames()
        {
            return blob.GetContainerNames();
        }

        public List<string> QueueNames()
        {
            return queue.GetQueueNames();
        }
    }
}
