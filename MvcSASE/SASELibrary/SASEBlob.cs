using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    class SASEBlob
    {
        private CloudBlobClient blobClient;
        private List<CloudBlobContainer> containerList = new List<CloudBlobContainer>();

        // Intended Constructor
        public SASEBlob(CloudStorageAccount account)
        {
            blobClient = account.CreateCloudBlobClient();

            IEnumerable<CloudBlobContainer> blobContainers = blobClient.ListContainers();
            foreach (CloudBlobContainer blobContainer in blobContainers)
            {
                containerList.Add(blobContainer);
            }
        }
        // Default Constructor
        public SASEBlob() { }

        // Returns a list of container names within the storage account
        public List<string> GetContainerNames()
        {
            List<string> names = new List<string>();

            foreach (CloudBlobContainer container in containerList)
                names.Add(container.Name);

            return names;
        }
    }
}
