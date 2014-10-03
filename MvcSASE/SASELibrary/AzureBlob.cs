using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SASELibrary
{
    public class AzureBlob
    {
        private readonly CloudBlobClient _blobClient;
        private readonly List<CloudBlobContainer> _containerList = new List<CloudBlobContainer>();
        private CloudBlobContainer _blobContainer;

        // Intended Constructor
        public AzureBlob(CloudStorageAccount account)
        {
            Uri endpoint = account.BlobEndpoint;
            _blobClient = new CloudBlobClient(endpoint, account.Credentials);
            //blobClient = account.CreateCloudBlobClient();

            IEnumerable<CloudBlobContainer> blobContainers = _blobClient.ListContainers();
            foreach (CloudBlobContainer blobContainer in blobContainers)
            {
                _containerList.Add(blobContainer);
            }
        }

        public AzureBlob()
        {
        }

        // Returns a list of container names within the storage account
        public IEnumerable<string> GetContainerNames()
        {
            return _containerList.Select(t => t.Name);
        }

        // Returns a list of blob items via their URI
        public IEnumerable<string> GetBlobItemNames(string container)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            _blobContainer.CreateIfNotExists();

            IEnumerable<IListBlobItem> blobItems = _blobContainer.ListBlobs();
            foreach (IListBlobItem blobItem in blobItems)
            {
                string fix = blobItem.Uri.AbsolutePath;
                fix = fix.Replace("%20", " ");
                yield return fix.Replace("%22", "\"");
            }
        }

        // Creates a new container with reference 'name'
        public bool CreateContainer(string name)
        {
            _blobContainer = _blobClient.GetContainerReference(name);
            bool created = _blobContainer.CreateIfNotExists();

            if (created)
                _containerList.Add(_blobContainer);

            return created;
        }

        // Creates a new blob block from a file on user's computer
        public void UploadBlockBlob(string container, string name, string filepath)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(name);

            blob.UploadFromFile(filepath, FileMode.Open);
        }

        // Create a new blob block from a byte array
        public void UploadBlockBlobBytes(string container, string name, byte[] file)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(name);

            blob.UploadFromByteArray(file, 0, file.Count());
        }

        // Create a new blob block from a file stream
        public void UploadBlockBlobStream(string container, string name, Stream file)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(name);

            blob.UploadFromStream(file);
        }

        // Retrieve a blob item's byte code
        public byte[] GetBlobBytes(string container, string item)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(item);
            blob.FetchAttributes();

            long fileByteLength = blob.Properties.Length;
            var fileContent = new byte[fileByteLength];

            for (int i = 0; i < fileByteLength; i++)
                fileContent[i] = 0x20;

            blob.DownloadToByteArray(fileContent, 0);

            return fileContent;
        }

        // Download a blob item to specified file path
        public void DownloadBlobItem(string container, string item, string filepath)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(item);

            blob.DownloadToFile(filepath, FileMode.CreateNew);
        }

        // Download a blob item as a Stream
        public Stream DownloadBlobStream(string container, string item)
        {
            Stream s = new MemoryStream();

            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(item);

            blob.DownloadToStream(s);
            s.Position = 0;

            return s;
        }

        public BlobInfo BlobInfo(string container, string item)
        {
            _blobContainer = _blobClient.GetContainerReference(container);
            CloudBlockBlob blob = _blobContainer.GetBlockBlobReference(item);

            blob.FetchAttributes();

            var blobinfo = new BlobInfo
            {
                BlobType = blob.Properties.BlobType.ToString(),
                Length = blob.Properties.Length.ToString(CultureInfo.InvariantCulture),
                LastModified = blob.Properties.LastModified.ToString(),
                BlobLocation = blob.Uri.ToString()
            };

            return blobinfo;
        }
    }

    public class BlobInfo
    {
        public string BlobType { get; set; }
        public string Length { get; set; }
        public string LastModified { get; set; }
        public string BlobLocation { get; set; }
    }
}