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
            SASEAccountService sase = new SASEAccountService(name, key);

            /********************************************
            // Attempt to create a new container
            string testCreateName = "createthis2";
            if (sase.SASECreateContainer(testCreateName))
                Console.WriteLine("Created new blob container named: " + testCreateName + '\n');
            else
                Console.WriteLine("The container name given either already exists or has inappropriate characters: " + testCreateName + '\n');

            // Attempt to upload a file from 'uploadPath' to blob cotainer 'uploadContainer'
            string uploadPath = @"C:\Users\Administrator\Desktop\tunes.txt";
            string uploadContainer = "createthis";
            Console.WriteLine("Attempting to upload file from " + uploadPath + "\nTo container: " + uploadContainer);
            if (sase.SASEUploadBlockBlob(uploadContainer, uploadPath))
                Console.WriteLine("Uploading succeeded!\n");
            else
                Console.WriteLine("Uploading failed...\n");

            // List all blob containers by name and their blob items
            Console.WriteLine("The blob containers on this storage account are:\n");
            foreach (string container in sase.SASEBlobContainerNames())
            {
                Console.WriteLine(container);
                foreach (string item in sase.SASEBlobItemNames(container))
                {
                    string itemName = "";
                    int slash1 = item.IndexOf("/");
                    int slash2 = item.IndexOf("/", slash1 + 1);

                    if (slash2 > 1)
                        itemName = item.Remove(slash1, slash2 + 1);

                    Console.WriteLine('\t' + itemName);                    
                }

                Console.WriteLine("");
            }
            ********************************************/
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

            /*
            // Enqueue a 'message' to 'queueName'
            Console.WriteLine("Attempting to enqueue the message: " + message + "\nTo queue: " + queueName);
            if (sase.SASEEnqueueMessage(queueName, message))
                Console.WriteLine("Succeeded!");
            else
                Console.WriteLine("Failed..");
            */



            // List all storage queues by name and their approximate message count
            Console.WriteLine("\nThe queue names on this storage account are:");
            foreach (string container in sase.SASEQueueNames())
            {
                Console.Write(container);
                Console.WriteLine("\t\tMessage Count: " + sase.SASEQueueMessageCount(container).ToString());
            }

            // End of program.  Keeps console window open.
            Console.WriteLine("\n\nPress 'Enter' to exit..");
            Console.ReadLine();
        }
    }
}
