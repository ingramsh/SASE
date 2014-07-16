using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //--SASE Blob Operations--//
        public List<string> SASEBlobContainerNames()
        {
            return blob.GetContainerNames();
        }
        public List<string> SASEBlobBlockNames(string container)
        {
            List<string> blockNames = new List<string>();

            //TODO:  Return list of 'blockNames' from 'container'
            return blockNames;
        }
        public void SASECreateContainer(string name)
        {
            //TODO:  Create a new blob container labeled 'name'
        }
        public void SASEUploadBlobBlock(string container, string filename)
        {
            //TODO:  Upload file from 'filename' to 'container'
        }

        //--SASE Queue Operations--//
        public List<string> SASEQueueNames()
        {
            return queue.GetQueueNames();
        }
        public void SASEEnqueueMessage(string queue, string message)
        {
            //TODO:  add 'message' to 'queue'
        }
        public string SASEDequeueMessage(string queue)
        {
            string message = null;
            
            //TODO: remove 'message' from front of 'queue' and return it
            return message;
        }
        public string SASEPeekMessage(string queue)
        {
            string message = null;

            //TODO: peek 'message' from front of 'queue' and return it
            return message;
        }


    }
}
