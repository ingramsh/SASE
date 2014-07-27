using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SASELibrary
{
    class Queue
    {
        CloudQueueClient queueClient;
        CloudQueue cloudQueue;
        Exception last;
        private List<CloudQueue> queueList = new List<CloudQueue>();

        // Intended Constructor
        public Queue(CloudStorageAccount account)
        {
            queueClient = account.CreateCloudQueueClient();

            IEnumerable<CloudQueue> cloudQueues = queueClient.ListQueues();
            foreach (CloudQueue cloudQueue in cloudQueues)
            {
                queueList.Add(cloudQueue);
            }
        }
        public Queue() { }

        // Returns a list of queues named within the storage account
        public IEnumerable<string> GetQueueNames()
        {
            return queueList.Select(q => q.Name);
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
        public Message PeekMessage(string name)
        {
            cloudQueue = queueClient.GetQueueReference(name);
            CloudQueueMessage peekMessage = cloudQueue.PeekMessage();
            return new Message()
            {
                MessageString = peekMessage.AsString,
                DequeueCount = peekMessage.DequeueCount.ToString(),

                InsertionTime = peekMessage.InsertionTime.ToString(),
                ExpirationTime = peekMessage.ExpirationTime.ToString(),
                NextVisibleTime = peekMessage.NextVisibleTime.ToString()
            };
        }
    }

    public class Message
    {
        public string MessageString { get; set; }
        public string DequeueCount { get; set; }
        public string InsertionTime { get; set; }
        public string ExpirationTime { get; set; }
        public string NextVisibleTime { get; set; }
    }
}
