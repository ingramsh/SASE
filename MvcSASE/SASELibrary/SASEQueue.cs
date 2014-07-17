using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    class SASEQueue
    {
        CloudQueueClient queueClient;
        CloudQueue cloudQueue;
        Exception last;
        private List<CloudQueue> queueList = new List<CloudQueue>();

        // Intended Constructor
        public SASEQueue(CloudStorageAccount account)
        {
            queueClient = account.CreateCloudQueueClient();

            IEnumerable<CloudQueue> cloudQueues = queueClient.ListQueues();
            foreach (CloudQueue cloudQueue in cloudQueues)
            {
                queueList.Add(cloudQueue);
            }
        }
        public SASEQueue() { }

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

        // Enqueue a message to 'queue'
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

        // Dequeue a message from the front of 'queue'
        public string DequeueMessage(string name, string message)
        {
            return "happy";
        }
    }
}
