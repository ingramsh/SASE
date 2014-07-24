using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    class AzureQueue : Queue
    {
        CloudQueueClient queueClient;
        CloudQueue cloudQueue;
        Exception last;
        private List<CloudQueue> queueList = new List<CloudQueue>();

        // Intended Constructor
        public AzureQueue(CloudStorageAccount account)
        {
            queueClient = account.CreateCloudQueueClient();

            IEnumerable<CloudQueue> cloudQueues = queueClient.ListQueues();
            foreach (CloudQueue cloudQueue in cloudQueues)
            {
                queueList.Add(cloudQueue);
            }
        }
        public AzureQueue() { }

        // Returns a list of queues named within the storage account
        public List<string> GetQueueNames()
        {
            List<string> names = new List<string>();

            foreach (CloudQueue queue in queueList)
                names.Add(queue.Name);

            return names;
        }

        // Returns the queue's message count
        public int GetMessageCount(string name)
        {
            int? count = null;

            cloudQueue = queueClient.GetQueueReference(name);
            cloudQueue.FetchAttributes();
            count = cloudQueue.ApproximateMessageCount;

            if (count != null)
                return (int)cloudQueue.ApproximateMessageCount;
            else
                return 0;
        }

        // Create a new storage queue
        public bool CreateQueue(string name)
        {
            bool created = false;

            cloudQueue = queueClient.GetQueueReference(name);
            created = cloudQueue.CreateIfNotExists();

            if (created)
                queueList.Add(cloudQueue);

            return created;
        }

        // Enqueue a message to queue
        public bool EnqueueMessage(string name, string message)
        {
            CloudQueueMessage queueMessage = new CloudQueueMessage(message);
            cloudQueue = queueClient.GetQueueReference(name);
            try
            {
                cloudQueue.AddMessage(queueMessage);
            }
            catch (Exception e)
            {
                last = e;
                return false;
            }

            return true;
        }

        // Dequeue a message from the front of queue
        public string DequeueMessage(string name)
        {
            string message;

            cloudQueue = queueClient.GetQueueReference(name);
            CloudQueueMessage getMessage = cloudQueue.GetMessage();

            message = getMessage.AsString;
            cloudQueue.DeleteMessage(getMessage);
            
            return message;
        }

        // Peek a message from the front of queue
        public List<string> PeekMessage(string name)
        {
            List<string> peek = new List<string>();

            cloudQueue = queueClient.GetQueueReference(name);
            CloudQueueMessage peekMessage = cloudQueue.PeekMessage();

            peek.Add(peekMessage.AsString);

            if (peekMessage.DequeueCount.ToString() == "")
                peek.Add("0");
            else
                peek.Add(peekMessage.DequeueCount.ToString());

            peek.Add(peekMessage.InsertionTime.ToString());
            peek.Add(peekMessage.ExpirationTime.ToString());
            peek.Add(peekMessage.NextVisibleTime.ToString());

            return peek;
        }
    }
}
