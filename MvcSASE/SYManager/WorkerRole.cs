using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using SASELibrary;
using System.Text.RegularExpressions;

namespace SYManager
{
    public class WorkerRole : RoleEntryPoint
    {
        private SASEAccountService sase;

        private string container = "sase-youtube-videos";

        private string queue_in = "sase-youtube-in";
        private string queue_out = "sase-youtube-out";
        private List<string> peek;

        private string message;
        public static readonly Regex YoutubeVideoRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("SYManager entry point called");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working");

                if (sase.SASEQueueMessageCount(queue_in) > 0)
                {
                    peek = sase.SASEPeekMessage(queue_in);

                    if (Convert.ToInt32(peek[1]) >= 2)
                        sase.SASEDequeueMessage(queue_in);
                    else
                    {
                        message = peek[0];

                        string url = message;

                        Match youtubeMatch = YoutubeVideoRegex.Match(url);

                        string id = string.Empty;

                        if (youtubeMatch.Success)
                        {
                            id = youtubeMatch.Groups[1].Value;
                            sase.SASEDequeueMessage(queue_in);

                                //TODO:  create 'SYWorker' to rip youtube video from url in 'message'

                                    //TODO:  retrieve io stream to upload to blob 'container'

                                        //TODO: enqueue complete message to 'queue_out'
                        }
                    }
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            string name = "daowna";
            string key = "wuG0USYr/U+x6i6r8KojOXfZOL5qWQQdAgDGnt2V+lSyyW2Rv74BY4IdJz+5i45pbBbz+5gH/eCcDpy7Fn9qwA==";

            sase = new SASEAccountService(name, key);
            sase.SASECreateContainer(container);
            sase.SASECreateQueue(queue_in);
            sase.SASECreateQueue(queue_out);
            
            return base.OnStart();
        }
    }
}
