using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace SASELibrary
{
    public class AzureQueue
    {
        private readonly CloudQueueClient _queueClient;
        private readonly List<CloudQueue> _queueList = new List<CloudQueue>();
        private CloudQueue _cloudQueue;

        // Intended Constructor
        public AzureQueue(CloudStorageAccount account)
        {
            _queueClient = account.CreateCloudQueueClient();

            IEnumerable<CloudQueue> cloudQueues = _queueClient.ListQueues();
            foreach (CloudQueue cloudQueue in cloudQueues)
            {
                _queueList.Add(cloudQueue);
            }
        }

        public AzureQueue()
        {
        }

        // Returns a list of queues named within the storage account
        public IEnumerable<string> GetQueueNames()
        {
            return _queueList.Select(q => q.Name);
        }

        // Returns the queue's message count
        public int GetMessageCount(string name)
        {
            int? count;

            _cloudQueue = _queueClient.GetQueueReference(name);
            _cloudQueue.FetchAttributes();
            count = _cloudQueue.ApproximateMessageCount;

            if (count != null)
                if (_cloudQueue.ApproximateMessageCount != null) return (int) _cloudQueue.ApproximateMessageCount;
            return 0;
        }

        // Create a new storage queue
        public bool CreateQueue(string name)
        {
            bool created;

            _cloudQueue = _queueClient.GetQueueReference(name);
            created = _cloudQueue.CreateIfNotExists();

            if (created)
                _queueList.Add(_cloudQueue);

            return created;
        }

        // Enqueue a message to queue
        public bool EnqueueMessage(string name, string message)
        {
            var queueMessage = new CloudQueueMessage(message);
            _cloudQueue = _queueClient.GetQueueReference(name);
            try
            {
                _cloudQueue.AddMessage(queueMessage);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // Dequeue a message from the front of queue
        public string DequeueMessage(string name)
        {
            string message;

            _cloudQueue = _queueClient.GetQueueReference(name);
            CloudQueueMessage getMessage = _cloudQueue.GetMessage();

            message = getMessage.AsString;
            _cloudQueue.DeleteMessage(getMessage);

            return message;
        }

        // Peek a message from the front of queue
        public Message PeekMessage(string name)
        {
            _cloudQueue = _queueClient.GetQueueReference(name);
            CloudQueueMessage peekMessage = _cloudQueue.PeekMessage();
            return new Message
            {
                MessageString = peekMessage.AsString,
                DequeueCount = peekMessage.DequeueCount.ToString(CultureInfo.InvariantCulture),
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