using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class AWSBlob : Blob
    {
        private AmazonS3Client m_client;
        private AmazonS3Client Client
        {
            get
            {
                return m_client ??
                    (m_client = new AmazonS3Client(this.AccessKey, this.SecretKey));
            }
        }
        public override IEnumerable<string> BlobInfo(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override bool CreateContainer(string name)
        {
            PutBucketRequest request = new PutBucketRequest
            {
                BucketName = name,
                BucketRegion = S3Region.US,         // set region to EU
                CannedACL = S3CannedACL.PublicRead  // make bucket publicly readable
            };
            return Client.PutBucket(request).HttpStatusCode == System.Net.HttpStatusCode.Accepted;
        }

        public override void DownloadBlobItem(string container, string item, string filepath)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = container,
                Key = item
            };
            Client.GetObject(request).WriteResponseStreamToFile(filepath);
        }

        public override System.IO.Stream DownloadBlobStream(string container, string item)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = container,
                Key = item
            };
            return Client.GetObject(request).ResponseStream;
        }

        public override byte[] GetBlobBytes(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetBlobItemNames(string container)
        {
            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = container
            };
            return Client.ListObjects(request).S3Objects.Select(x=> DecodeURL(x.Key));
        }

        public override IEnumerable<string> GetContainerNames()
        {
            throw new NotImplementedException();
        }

        public override void UploadBlockBlob(string container, string name, string filepath)
        {
            throw new NotImplementedException();
        }

        public override void UploadBlockBlobBytes(string container, string name, byte[] file)
        {
            throw new NotImplementedException();
        }

        public override void UploadBlockBlobStream(string container, string name, System.IO.Stream file)
        {
            throw new NotImplementedException();
        }
    }
}
