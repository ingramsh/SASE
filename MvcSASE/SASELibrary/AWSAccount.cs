using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class AWSAccount : Account 
    {
        public override List<string> SASEBlobContainerNames()
        {
            throw new NotImplementedException();
        }

        public override List<string> SASEBlobInfo(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override List<string> SASEBlobItemNames(string container)
        {
            throw new NotImplementedException();
        }

        public override List<string> SASEBlobItems(string container)
        {
            throw new NotImplementedException();
        }

        public override int SASEContainerCount()
        {
            throw new NotImplementedException();
        }

        public override bool SASECreateContainer(string name)
        {
            throw new NotImplementedException();
        }

        public override bool SASECreateQueue(string name)
        {
            throw new NotImplementedException();
        }

        public override string SASEDequeueMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override bool SASEDownloadBlobBlock(string container, string item, string filepath)
        {
            throw new NotImplementedException();
        }

        public override byte[] SASEDownloadBlobBytes(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream SASEDownloadBlobStream(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override bool SASEEnqueueMessage(string name, string message)
        {
            throw new NotImplementedException();
        }

        public override List<string> SASEPeekMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override int SASEQueueCount()
        {
            throw new NotImplementedException();
        }

        public override int SASEQueueMessageCount(string name)
        {
            throw new NotImplementedException();
        }

        public override List<string> SASEQueueNames()
        {
            throw new NotImplementedException();
        }

        public override bool SASEUploadBlockBlob(string container, string filepath)
        {
            throw new NotImplementedException();
        }

        public override bool SASEUploadBlockBlobBytes(string container, string name, byte[] file)
        {
            throw new NotImplementedException();
        }

        public override bool SASEUploadBlockBlobStream(string container, string name, System.IO.Stream file)
        {
            throw new NotImplementedException();
        }
    }
}
