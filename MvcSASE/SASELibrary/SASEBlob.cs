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
        private CloudBlobContainer blobContainer;
        private Uri endpoint;
        private List<CloudBlobContainer> containerList = new List<CloudBlobContainer>();

        // Intended Constructor
        public SASEBlob(CloudStorageAccount account)
        {
            endpoint = account.BlobEndpoint;
            blobClient = new CloudBlobClient(endpoint, account.Credentials);
            //blobClient = account.CreateCloudBlobClient();

            IEnumerable<CloudBlobContainer> blobContainers = blobClient.ListContainers();
            foreach (CloudBlobContainer blobContainer in blobContainers)
            {
                containerList.Add(blobContainer);
            }
        }
        public SASEBlob() { }

        // Returns a list of container names within the storage account
        public List<string> GetContainerNames()
        {
            List<string> names = new List<string>();

            foreach (CloudBlobContainer container in containerList)
                names.Add(container.Name);

            return names;
        }

        // Returns a list of blob items via their URI
        public List<string> GetBlobItemNames(string container)
        {
            List<string> names = new List<string>();

            blobContainer = blobClient.GetContainerReference(container);
            blobContainer.CreateIfNotExists();

            IEnumerable<IListBlobItem> blobItems = blobContainer.ListBlobs();
            foreach (IListBlobItem blobItem in blobItems)
                names.Add(blobItem.Uri.AbsolutePath);

            return names;
        }

        // Creates a new container with reference 'name'
        public bool CreateContainer(string name)
        {
            bool created = false;

            blobContainer = blobClient.GetContainerReference(name);
            created = blobContainer.CreateIfNotExists();

            if (created)
                containerList.Add(blobContainer);

            return created;
        }

        // Creates a new blob block from a file on user's computer
        public void UploadBlockBlob(string container, string name, string filepath)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(name);

            blob.UploadFromFile(filepath, System.IO.FileMode.Open);
        }
    }
}
