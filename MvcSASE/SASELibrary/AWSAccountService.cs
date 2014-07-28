using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using System.Net;
using System.IO;
using Amazon.Runtime;
namespace SASELibrary
{
    public class AWSAccountService : AccountService
    {
        private IAmazonS3 client;
        public IAmazonS3 Client
        {
            get
            {
                return client ?? (client = new AmazonS3Client(this.Creds, Amazon.RegionEndpoint.USEast1));
            }
        }
        public BasicAWSCredentials Creds
        {
            get
            {
                return new BasicAWSCredentials(this.storageAccount, this.storageKey);
            }
        }
        public override IEnumerable<string> BlobContainerNames()
        {

            return this.Buckets.Select(b => b.BucketName);
        }
        private IEnumerable<S3Bucket> Buckets
        {
            get
            {
                return Client.ListBuckets().Buckets;
            }
        }
        public override BlobInfo BlobInfo(string container, string item)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = container,
                Key = item
            };
            GetObjectResponse response = Client.GetObject(request);
            BlobInfo blobinfo = new BlobInfo()
            {
                BlobLocation = response.Key,
                LastModified = response.LastModified.ToString(),
                BlobType = "Amazon Blob",
                Length = response.ContentLength.ToString()
            };
            return blobinfo;
        }

        public override IEnumerable<string> BlobItemNames(string container)
        {
            S3Bucket bucket = this.Buckets.Where(b => b.BucketName == container).FirstOrDefault();
            
            ListObjectsRequest request = new ListObjectsRequest()
            {
                BucketName = container
            };
            ListObjectsResponse response = Client.ListObjects(request);
            return response.S3Objects.Select(o => o.Key);
        }

        public override IEnumerable<string> BlobItems(string container)
        {
            return BlobItemNames(container);
        }

        public override bool CreateContainer(string name)
        {
            PutBucketRequest request = new PutBucketRequest()
            {
                BucketName = name
            };
            return Client.PutBucket(request).HttpStatusCode == HttpStatusCode.Accepted;
        }

        public override bool CreateQueue(string name)
        {
            throw new NotImplementedException();
        }

        public override string DequeueMessage(string name)
        {
            throw new NotImplementedException();
        }

        public override byte[] DownloadBlobBytes(string container, string item)
        {
            GetObjectResponse response = this.GetDownloadResponse(container, item);
            byte[] byteArray = new byte[response.ContentLength];
            response.ResponseStream.Read(byteArray, 0, (int)response.ContentLength);
            return byteArray;
        }

        public override System.IO.Stream DownloadBlobStream(string container, string item)
        {
            return this.GetDownloadResponse(container, item).ResponseStream;
        }
        private GetObjectResponse GetDownloadResponse(string container, string item)
        {
            GetObjectRequest request = new GetObjectRequest()
            {
                BucketName = container,
                Key = item
            };
            return Client.GetObject(request);
        }
        public override bool EnqueueMessage(string name, string message)
        {
            return true;
        }

        public override Message PeekMessage(string name)
        {
            return null;
        }

        public override int QueueCount()
        {
            return 0;
        }

        public override int QueueMessageCount(string name)
        {
            return 0;
        }

        public override IEnumerable<string> QueueNames()
        {
            return null;
        }

        public override bool UploadBlockBlob(string container, string filepath)
        {

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = container,
                FilePath = filepath,
                Key = Path.GetFileName(filepath)
            };
            return Client.PutObject(request).HttpStatusCode == HttpStatusCode.Accepted;
        }

        public override bool UploadBlockBlobBytes(string container, string name, byte[] file)
        {
            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = container,
                Key = name
            };
            request.InputStream.Write(file, 0, file.Length);
            return Client.PutObject(request).HttpStatusCode == HttpStatusCode.Accepted;
        }

        public override bool UploadBlockBlobStream(string container, string name, System.IO.Stream file)
        {

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = container,
                Key = name,
                InputStream = file
            };
            return Client.PutObject(request).HttpStatusCode == HttpStatusCode.Accepted;
        }
    }
}
