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
    public class SASEAccountService
    {
        private StorageCredentials creds;
        private CloudStorageAccount account;
        private SASEBlob blob;
        private SASEQueue queue;

        // Intended Constructor
        public SASEAccountService(string name, string key)
        {
            creds = new StorageCredentials(name, key);
            account = new CloudStorageAccount(creds, false);
            blob = new SASEBlob(account);
            queue = new SASEQueue(account);
        }

        //---SASE Blob Operations---//
        public List<string> SASEBlobContainerNames()
        {
            return blob.GetContainerNames();
        }
        public List<string> SASEBlobItemNames(string container)
        {            
            return blob.GetBlobItemNames(container);
        }
        public bool SASECreateContainer(string name)
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
        public bool SASEUploadBlockBlob(string container, string filepath)
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

            filecheck = Path.GetFileNameWithoutExtension(filepath);
            if (filecheck == "")
            {
                //TODO:  Create exception for submitting a filepath to a directory instead of a file
                return false;
            }

            foreach (string blobName in blob.GetBlobItemNames(container))
            {
                string itemName = "";
                int slash1 = blobName.IndexOf("/");
                int slash2 = blobName.IndexOf("/", slash1 + 1);

                if (slash2 > 1)
                    itemName = blobName.Remove(slash1, slash2 + 1);

                if (itemName == filecheck)
                {
                    //TODO:  Create exception for submitting a file name that already exists in the given container
                    return false;
                }
            }

            blob.UploadBlockBlob(container, filecheck, filepath);
            return true;
        }
        private bool ContainerNameExists(string name)
        {
            foreach (string container in blob.GetContainerNames())
                if (container == name)
                    return true;

            return false;
        }

        //---SASE Queue Operations---//
        public List<string> SASEQueueNames()
        {
            return queue.GetQueueNames();
        }
        public int SASEQueueMessageCount(string name)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return -1;
            }

            return queue.GetMessageCount(name);
        }
        public bool SASECreateQueue(string name)
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
        public bool SASEEnqueueMessage(string name, string message)
        {
            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for submitting a non-existent queue
                return false;
            }

            return queue.EnqueueMessage(name, message);
        }
        public string SASEDequeueMessage(string name)
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
        public List<string> SASEPeekMessage(string name)
        {
            List<string> peek = new List<string>();

            if (!QueueNameExists(name))
            {
                //TODO:  Create exception for attempting to peek a queue that does not exist
                return peek;
            }
            if (queue.GetMessageCount(name) == 0)
            {
                //TODO:  Create exception for attempting to peek and empty queue
                return peek;
            }

            peek = queue.PeekMessage(name);

            return peek;
        }
        private bool QueueNameExists(string name)
        {
            foreach (string queueLabel in queue.GetQueueNames())
                if (queueLabel == name)
                    return true;

            return false;
        }
    }
}
