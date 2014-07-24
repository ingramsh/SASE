using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace SASELibrary
{
    public class AzureAccount : Account
    {
        private StorageCredentials creds;
        private CloudStorageAccount account;
        private AzureBlob blob;
        private AzureQueue queue;

        // Intended Constructor
        public AzureAccount(string name, string key)
        {
            creds = new StorageCredentials(name, key);
            account = new CloudStorageAccount(creds, false);
            blob = new AzureBlob(account);
            queue = new AzureQueue(account);
        }
        public AzureAccount() { }

        #region SASE BLOB OPPs
        //---SASE Blob Operations---//
        public override List<string> BlobContainerNames()
        {
            return blob.GetContainerNames();
        }
        public override int ContainerCount()
        {
            return this.BlobContainerNames().Count;
        }
        public override List<string> BlobItemNames(string container)
        {
            List<string> blobNames = new List<String>();
            foreach (string item in blob.GetBlobItemNames(container))
            {
                string itemName = "";
                int slash1 = item.IndexOf("/");
                int slash2 = item.IndexOf("/", slash1 + 1);

                if (slash2 > 1)
                    itemName = item.Remove(slash1, slash2 + 1);

                blobNames.Add(itemName);
            }

            return blobNames;
        }
        public override List<string> BlobItems(string container)
        {
            return blob.GetBlobItemNames(container);
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
            
            return blob.CreateContainer(name);
        }
        public override bool UploadBlockBlob(string container, string filepath)
        {
            string filecheck;

            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return false;
            }

            filecheck = Path.GetDirectoryName(filepath);
            if (filecheck == "")
            {
                //TODO:  Create exception for submitting an invalid filepath
                return false;
            }

            filecheck = Path.GetFileName(filepath);
            if (filecheck == "")
            {
                //TODO:  Create exception for submitting a filepath to a directory instead of a file
                return false;
            }
            /*
            if (BlobItemExists(container, filecheck))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            blob.UploadBlockBlob(container, filecheck, filepath);

            return true;
        }
        public override bool UploadBlockBlobBytes(string container, string name, byte[] file)
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

            blob.UploadBlockBlobBytes(container, name, file);

            return true;
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

            blob.UploadBlockBlobStream(container, name, file);

            return true;
        }
        public override bool DownloadBlobBlock(string container, string item, string filepath)
        {
            string filecheck = Path.GetDirectoryName(filepath);
            if (filecheck == "")
            {
                //TODO:  Create exception for submitting an invalid filepath
                return false;
            }
            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return false;
            }
            /*
            if (!BlobItemExists(container, item))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return false;
            }
            */

            blob.DownloadBlobItem(container, item, filepath);

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

            return blob.DownloadBlobStream(container, item);
        }
        public override byte[] DownloadBlobBytes(string container, string item)
        {
            byte[] byteArray;

            if (!ContainerNameExists(container))
            {
                //TODO:  Create exception for submitting a non-existent container
                return byteArray = null;
            }
            /*
            if (!BlobItemExists(container, item))
            {
                //TODO:  Create exception for submitting a non-existent blob item
                Console.WriteLine("INVALID BLOB ITEM");
                return byteArray = null;
            }
            */

            return byteArray = blob.GetBlobBytes(container, item);
        }
        public override List<string> BlobInfo(string container, string item)
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

            return blob.BlobInfo(container, item);
        }
        private bool ContainerNameExists(string name)
        {
            foreach (string container in blob.GetContainerNames())
                if (container == name)
                    return true;

            return false;
        }
        private bool BlobItemExists(string container, string item)
        {
            foreach (string blobName in blob.GetBlobItemNames(container))
            {
                /*string itemName = "";
                int slash1 = blobName.IndexOf("/");
                int slash2 = blobName.IndexOf("/", slash1 + 1);

                if (slash2 > 1)
                    itemName = blobName.Remove(slash1, slash2 + 1);

                for (int i = 0; i < blobName.Count(); )

                    if (itemName == item)
                    {
                        return true;
                    }*/
                if (blobName == item)
                    return true;
            }

            return false;
        }
        #endregion

        #region SASE QUEUE OPPs
        //---SASE Queue Operations---//
        public override List<string> QueueNames()
        {
            return queue.GetQueueNames();
        }
        public override int QueueCount()
        {
            return this.QueueNames().Count;
        }
        public override int QueueMessageCount(string name)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return -1;
            }

            return queue.GetMessageCount(name);
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

            return queue.CreateQueue(name);
        }
        public override bool EnqueueMessage(string name, string message)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return false;
            }

            return queue.EnqueueMessage(name, message);
        }
        public override string DequeueMessage(string name)
        {
            string message = null;

            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for attempting to dequeue a queue that does not exist
                return message;
            }
            if (queue.GetMessageCount(name) == 0)
            {
                //TODO:  Create exception for attempting to dequeue and empty queue
                return message;
            }

            message = queue.DequeueMessage(name);

            return message;
        }
        public override List<string> PeekMessage(string name)
        {
            List<string> peek = new List<string>();

            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for attempting to peek a queue that does not exist
                for (int i = 0; i < 5; i++)
                    peek.Add("");

                return peek;
            }
            if (queue.GetMessageCount(name) == 0)
            {
                //TODO:  Create exception for attempting to peek and empty queue
                for (int i = 0; i < 5; i++)
                    peek.Add("");

                return peek;
            }

            return queue.PeekMessage(name);

            //return peek;
        }
        private bool QueueNameExists(string name)
        {
            foreach (string queueLabel in queue.GetQueueNames())
                if (queueLabel == name)
                    return true;

            return false;
        }
        #endregion
    }
}
