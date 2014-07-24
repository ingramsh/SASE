using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SASELibrary
{
    public class AzureBlob : Blob
    {
        private CloudBlobClient blobClient;        
        private CloudBlobContainer blobContainer;
        private Uri endpoint;
        private List<CloudBlobContainer> containerList = new List<CloudBlobContainer>();

        // Intended Constructor
        public AzureBlob(CloudStorageAccount account)
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
        public AzureBlob() { }

        // Returns a list of container names within the storage account
        public override List<string> GetContainerNames()
        {
            List<string> names = new List<string>();

            foreach (CloudBlobContainer container in containerList)
                names.Add(container.Name);

            return names;
        }

        // Returns a list of blob items via their URI
        public override List<string> GetBlobItemNames(string container)
        {
            List<string> names = new List<string>();

            blobContainer = blobClient.GetContainerReference(container);
            blobContainer.CreateIfNotExists();

            IEnumerable<IListBlobItem> blobItems = blobContainer.ListBlobs();
            foreach (IListBlobItem blobItem in blobItems)
            {
                
                names.Add(blobItem.Uri.AbsolutePath.ToString().Replace("%20", " "));
            }

            return names;
        }

        // Creates a new container with reference 'name'
        public override bool CreateContainer(string name)
        {
            bool created = false;

            blobContainer = blobClient.GetContainerReference(name);
            created = blobContainer.CreateIfNotExists();

            if (created)
                containerList.Add(blobContainer);

            return created;
        }

        // Creates a new blob block from a file on user's computer
        public override void UploadBlockBlob(string container, string name, string filepath)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(name);

            blob.UploadFromFile(filepath, System.IO.FileMode.Open);
        }

        // Create a new blob block from a byte array
        public override void UploadBlockBlobBytes(string container, string name, byte[] file)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(name);

            blob.UploadFromByteArray(file, 0, file.Count());
        }

        // Create a new blob block from a file stream
        public override void UploadBlockBlobStream(string container, string name, Stream file)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(name);

            blob.UploadFromStream(file);
        }

        // Retrieve a blob item's byte code
        public override byte[] GetBlobBytes(string container, string item)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);
            blob.FetchAttributes();

            long fileByteLength = blob.Properties.Length;
            byte[] fileContent = new byte[fileByteLength];
            
            for (int i = 0; i < fileByteLength; i++)
                fileContent[i] = 0x20;

            blob.DownloadToByteArray(fileContent, 0);

            return fileContent;
        }

        // Download a blob item to specified file path
        public override void DownloadBlobItem(string container, string item, string filepath)
        {
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);

            blob.DownloadToFile(filepath, System.IO.FileMode.CreateNew);
        }

        // Download a blob item as a Stream
        public override Stream DownloadBlobStream(string container, string item)
        {
            Stream s = new MemoryStream();

            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);

            blob.DownloadToStream(s);
            s.Position = 0;

            return s;
        }

        public override List<string> BlobInfo(string container, string item)
        {
            List<string> attributes = new List<string>();
            blobContainer = blobClient.GetContainerReference(container);
            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(item);

            blob.FetchAttributes();

            attributes.Add(blob.Properties.BlobType.ToString());
            attributes.Add(blob.Properties.Length.ToString());
            attributes.Add(blob.Properties.LastModified.ToString());
            attributes.Add(blob.Uri.ToString());

            return attributes;
        }
    }
}
