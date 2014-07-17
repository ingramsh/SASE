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
            string name = "daowna";
            string key = "wuG0USYr/U+x6i6r8KojOXfZOL5qWQQdAgDGnt2V+lSyyW2Rv74BY4IdJz+5i45pbBbz+5gH/eCcDpy7Fn9qwA==";
            SASEAccountService sase = new SASEAccountService(name, key);

            Console.WriteLine("The blob containers on this storage account are:");           
            foreach (string container in sase.SASEBlobContainerNames())
            {
                Console.WriteLine(container);
            }

            Console.WriteLine("");
            Console.WriteLine("The queue names on this storage account are:");
            foreach (string container in sase.SASEQueueNames())
            {
                Console.WriteLine(container);
            }

            // End of program.  Keeps console window open.
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Press 'Enter' to exit..");
            Console.ReadLine();
        }
    }
}
