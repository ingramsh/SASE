using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class AWSAccountService : AccountService
    {
        public override IEnumerable<string> BlobContainerNames()
        {
            throw new NotImplementedException();
        }

        public override BlobInfo BlobInfo(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> BlobItemNames(string container)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> BlobItems(string container)
        {
            throw new NotImplementedException();
        }

        public override bool CreateContainer(string name)
        {
            throw new NotImplementedException();
        }

        public override bool CreateQueue(string name)
        {
            throw new NotImplementedException();
        }

        public override string DequeueMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override bool DownloadBlobBlock(string container, string item, string filepath)
        {
            throw new NotImplementedException();
        }

        public override byte[] DownloadBlobBytes(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream DownloadBlobStream(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override bool EnqueueMessage(string name, string message)
        {
            throw new NotImplementedException();
        }

        public override Message PeekMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override int QueueCount()
        {
            throw new NotImplementedException();
        }

        public override int QueueMessageCount(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> QueueNames()
        {
            throw new NotImplementedException();
        }

        public override bool UploadBlockBlob(string container, string filepath)
        {
            throw new NotImplementedException();
        }

        public override bool UploadBlockBlobBytes(string container, string name, byte[] file)
        {
            throw new NotImplementedException();
        }

        public override bool UploadBlockBlobStream(string container, string name, System.IO.Stream file)
        {
            throw new NotImplementedException();
        }
    }
}
