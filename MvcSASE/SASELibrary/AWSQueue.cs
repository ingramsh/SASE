using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class AWSQueue: Queue
    {

        public override bool CreateQueue(string name)
        {
            throw new NotImplementedException();
        }

        public override string DequeueMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override bool EnqueueMessage(string name, string message)
        {
            throw new NotImplementedException();
        }

        public override int GetMessageCount(string name)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetQueueNames()
        {
            throw new NotImplementedException();
        }

        public override List<string> PeekMessage(string name)
        {
            throw new NotImplementedException();
        }
    }
}
