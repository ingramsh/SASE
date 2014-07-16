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
        // Default Constructor
        public SASEQueue() { }

        // Returns a list of queues named within the storage account
        public List<string> GetQueueNames()
        {
            List<string> names = new List<string>();

            foreach (CloudQueue queue in queueList)
                names.Add(queue.Name);

            return names;
        }
    }
}
