using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
namespace SASELibrary
{
    public class AWSAccount : Account 
    {
        private AmazonS3Client client;
        public override List<string> BlobContainerNames()
        {
            throw new NotImplementedException();
        }

        public override List<string> BlobInfo(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override List<string> BlobItemNames(string container)
        {
            throw new NotImplementedException();
        }

        public override List<string> BlobItems(string container)
        {
            throw new NotImplementedException();
        }

        public override int ContainerCount()
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

        public override List<string> PeekMessage(string name)
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

        public override List<string> QueueNames()
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
