using SASELibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSASE
{
    class Program
    {
        static void Main(string[] args)
        {
            // Raw cloud storage account credentials
            string name = "daowna";
            string key = "wuG0USYr/U+x6i6r8KojOXfZOL5qWQQdAgDGnt2V+lSyyW2Rv74BY4IdJz+5i45pbBbz+5gH/eCcDpy7Fn9qwA==";

            // Creates account service class from SASE library
            Account sase = new Account(name, key);

            /********************************************
            // Attempt to create a new container
            string testCreateName = "createthis2";
            if (sase.SASECreateContainer(testCreateName))
                Console.WriteLine("Created new blob container named: " + testCreateName + '\n');
            else
                Console.WriteLine("The container name given either already exists or has inappropriate characters: " + testCreateName + '\n');

            // Attempt to upload a file from 'uploadPath' to blob cotainer 'uploadContainer'
            string uploadPath = @"C:\Users\Administrator\Desktop\Cloud.png";
            string uploadContainer = "createthis";
            Console.WriteLine("Attempting to upload file from " + uploadPath + "\nTo container: " + uploadContainer);
            if (sase.SASEUploadBlockBlob(uploadContainer, uploadPath))
                Console.WriteLine("Uploading succeeded!\n");
            else
                Console.WriteLine("Uploading failed...\n");       

            // Attempt to download a file 'blobItem' from 'containerName' to 'downloadPath'
            string blobItem = "Cloud.png";
            string containerName = "createthis";
            string downloadPath = @"C:\Users\Administrator\Desktop\Downloads\Cloud.png";

            Console.WriteLine("Attempting to download blob item: " + blobItem + "\nFrom container: " + containerName + "\nTo file path: " + downloadPath);
            if (sase.SASEDownloadBlobBlock(containerName, blobItem, downloadPath))
                Console.WriteLine("Success!\n");
            else
                Console.WriteLine("Failure.\n");
            ********************************************/

            // List all blob containers by name and their blob items
            Console.WriteLine("The blob containers and their contents on this storage account are:\n");
            foreach (string container in sase.SASEBlobContainerNames())
            {
                Console.WriteLine(container);
                /*foreach (string item in sase.SASEBlobItemNames(container))
                {
                    string itemName = "";
                    int slash1 = item.IndexOf("/");
                    int slash2 = item.IndexOf("/", slash1 + 1);

                    if (slash2 > 1)
                        itemName = item.Remove(slash1, slash2 + 1);

                    Console.WriteLine('\t' + itemName);                    
                }*/
                foreach (string item in sase.SASEBlobItemNames(container))
                    Console.WriteLine('\t' + item);

                Console.WriteLine("");
            }

            //^^Blobs^^===============================================================================================================vvQueuesvv//
            
            string queueName = "sase-test-queue";
            string message = "this is a test MeSsAgE for the SASE Service queue";

            /********************************************
            // Create a new storage queue titeled with 'queueName'
            string queueName = "sase-test-queue";
            if (sase.SASECreateQueue(queueName))
                Console.WriteLine("Created a new queue named: " + queueName);
            else
                Console.WriteLine("Failed to create a new queue named: " + queueName);            
            ********************************************/

            // Enqueue a 'message' to 'queueName'
            Console.WriteLine("\n\nAttempting to enqueue the message: " + message + "\nTo queue: " + queueName);
            if (sase.SASEEnqueueMessage(queueName, message))
                Console.WriteLine("Succeeded!\n");
            else
                Console.WriteLine("Failed..\n");
            
            // Peek the first message of 'queueName'
            List<string> peekMessage = sase.SASEPeekMessage(queueName);
            

            if (peekMessage.Count() > 0)
                Console.WriteLine("Peek Message: " + peekMessage.ElementAt(0));
            if (peekMessage.Count() > 1)
                Console.WriteLine("Dequeue Count: \t\t" + peekMessage.ElementAt(1));
            if (peekMessage.Count() > 2)
                Console.WriteLine("Insertion Time: \t" + peekMessage.ElementAt(2));
            if (peekMessage.Count() > 3)
                Console.WriteLine("Expiration Time: \t" + peekMessage.ElementAt(3));
            if (peekMessage.Count() > 4)
                Console.WriteLine("Next Visible In: \t" + peekMessage.ElementAt(4));
            Console.WriteLine('\n');
            

            // Dequeue the first message of 'queueName'
            string dequeueMessage = null;
            Console.WriteLine("Attempting to dequeue message from: " + queueName);
            dequeueMessage = sase.SASEDequeueMessage(queueName);

            if (dequeueMessage == null)
                Console.WriteLine("Failed: " + dequeueMessage);
            else
                Console.WriteLine("Succeeded: " + dequeueMessage);

            // List all storage queues by name and their approximate message count
            Console.WriteLine("\nThe queue names on this storage account are:");
            foreach (string container in sase.SASEQueueNames())
            {
                Console.Write(container);
                Console.WriteLine("\t\tMessage Count: " + sase.SASEQueueMessageCount(container).ToString());
            }

            // End of program.  Keeps console window open.
            Console.WriteLine("\n\nThe end.  Press 'Enter' to exit..");
            Console.ReadLine();
        }
    }
}
