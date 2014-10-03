using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace SASELibrary
{
    public class AzureAccountService : AccountService
    {
        private CloudStorageAccount _account;
        private AzureBlob _blob;
        private StorageCredentials _creds;
        private AzureQueue _queue;

        #region SASE CONSTRUCTOR AND CONTROLLER PROPERTIES

        [NotMapped]
        public StorageCredentials Creds
        {
            get { return _creds ?? (_creds = new StorageCredentials(storageAccount, storageKey)); }
        }

        [NotMapped]
        public CloudStorageAccount Account
        {
            get { return _account ?? (_account = new CloudStorageAccount(Creds, false)); }
        }

        [NotMapped]
        public AzureBlob BlobController
        {
            get { return _blob ?? (_blob = new AzureBlob(Account)); }
        }

        [NotMapped]
        public AzureQueue QueueController
        {
            get { return _queue ?? (_queue = new AzureQueue(Account)); }
        }

        #endregion

        #region SASE BLOB OPPs

        //---SASE Blob Operations---//
        public override IEnumerable<string> BlobContainerNames()
        {
            return BlobController.GetContainerNames();
        }

        /*public override int ContainerCount()
        {
            return this.BlobContainerNames().Count;
        }*/

        public override IEnumerable<string> BlobItemNames(string container)
        {
            var blobNames = new List<String>();
            foreach (string item in BlobController.GetBlobItemNames(container))
            {
                string itemName = "";
                int slash1 = item.IndexOf("/", StringComparison.Ordinal);
                int slash2 = item.IndexOf("/", slash1 + 1, StringComparison.Ordinal);

                if (slash2 > 1)
                    itemName = item.Remove(slash1, slash2 + 1);

                blobNames.Add(itemName);
            }

            return blobNames;
        }

        public override IEnumerable<string> BlobItems(string container)
        {
            return BlobController.GetBlobItemNames(container);
        }

        public override bool CreateContainer(string name)
        {
            if (ContainerNameExists(name))
            {
                //TODO:  Create exception for container name already exists
                return false;
            }
            if (name.Length < 3 || name.Length > 63 || !Regex.IsMatch(name, @"^[a-z0-9]+(-[a-z0-9]+)*$"))
            {
                //TODO:  Create exception for invalid container name characters or length
                return false;
            }

            return BlobController.CreateContainer(name);
        }

        public override bool UploadBlockBlobStream(string container, string name, Stream file)
        {
            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return false;
            }
            /*
            if (!BlobItemExists(container, name))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            BlobController.UploadBlockBlobStream(container, name, file);

            return true;
        }

        public override Stream DownloadBlobStream(string container, string item)
        {
            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return null;
            }
            /*
            if (!BlobItemExists(container, item))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            return BlobController.DownloadBlobStream(container, item);
        }

        public override byte[] DownloadBlobBytes(string container, string item)
        {
            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return null;
            }
            /*
            if (!BlobItemExists(container, item))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            return BlobController.GetBlobBytes(container, item);
        }

        public override BlobInfo BlobInfo(string container, string item)
        {
            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return null;
            }
            /*
            if (!BlobItemExists(container, item))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            return BlobController.BlobInfo(container, item);
        }

        private bool ContainerNameExists(string name)
        {
            foreach (string container in BlobController.GetContainerNames())
                if (container == name)
                    return true;

            return false;
        }

        #endregion

        #region SASE QUEUE OPPs

        //---SASE Queue Operations---//
        public override IEnumerable<string> QueueNames()
        {
            return QueueController.GetQueueNames();
        }

        public override int QueueCount()
        {
            return QueueNames().ToList().Count;
        }

        public override int QueueMessageCount(string name)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return -1;
            }

            return QueueController.GetMessageCount(name);
        }

        public override bool CreateQueue(string name)
        {
            if (QueueNameExists(name))
            {
                //TODO:  Create exception for queue name already exists
                return false;
            }
            if (name.Length < 3 || name.Length > 63 || !Regex.IsMatch(name, @"^[a-z0-9]+(-[a-z0-9]+)*$"))
            {
                //TODO:  Create exception for invalid queue name characters or length
                return false;
            }

            return QueueController.CreateQueue(name);
        }

        public override bool EnqueueMessage(string name, string message)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return false;
            }

            return QueueController.EnqueueMessage(name, message);
        }

        public override string DequeueMessage(string name)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for attempting to dequeue a queue that does not exist
                return null;
            }
            if (QueueController.GetMessageCount(name) == 0)
            {
                //TODO:  Create exception for attempting to dequeue and empty queue
                return null;
            }

            var message = QueueController.DequeueMessage(name);

            return message;
        }

        public override Message PeekMessage(string name)
        {
            //List<string> peek = new List<string>();
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for attempting to peek a queue that does not exist  
                return null;
            }
            if (QueueController.GetMessageCount(name) == 0)
            {
                //TODO:  Create exception for attempting to peek and empty queue
                return null;
            }

            QueueController.PeekMessage(name);

            return QueueController.PeekMessage(name);
        }

        private bool QueueNameExists(string name)
        {
            foreach (string queueLabel in QueueController.GetQueueNames())
                if (queueLabel == name)
                    return true;

            return false;
        }

        #endregion
    }
}