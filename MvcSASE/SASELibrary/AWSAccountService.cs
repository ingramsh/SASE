using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using System.Net;
using System.IO;
using Amazon.Runtime;
using Amazon.SQS.Model;
namespace SASELibrary
{
    public class AWSAccountService : AccountService
    {
        private IAmazonS3 client;
        private IAmazonSQS sqsClient;
        public IAmazonS3 Client
        {
            get
            {
                return client ?? (client = new AmazonS3Client(this.Creds, Amazon.RegionEndpoint.USEast1));
            }
        }
        public IAmazonSQS SQSClient
        {
            get
            {
                return sqsClient ?? (sqsClient = new AmazonSQSClient(Creds, Amazon.RegionEndpoint.USWest2));
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
            return Client.PutBucket(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override bool CreateQueue(string name)
        {
            CreateQueueRequest request = new CreateQueueRequest()
            {
                QueueName = name
            };
            return SQSClient.CreateQueue(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override string DequeueMessage(string name)
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest()
            {
                QueueUrl = name
            };
            
            Amazon.SQS.Model.Message message = SQSClient.ReceiveMessage(request).Messages[0];
            DeleteMessageRequest deleterequest = new DeleteMessageRequest()
            {
                QueueUrl = name,
                ReceiptHandle = message.ReceiptHandle
            };
            SQSClient.DeleteMessage(deleterequest);
            return message.Body;
        }

        public override byte[] DownloadBlobBytes(string container, string item)
        {
            GetObjectResponse response = this.GetDownloadResponse(container, item);
            byte[] byteArray = new byte[response.ContentLength];
            using (BinaryReader reader = new BinaryReader(response.ResponseStream))
            {
                byteArray = reader.ReadBytes((int)response.ContentLength);
            }
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
            SendMessageRequest request = new SendMessageRequest()
            {
                MessageBody = message,
                QueueUrl = name
            };
            return SQSClient.SendMessage(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override Message PeekMessage(string name)
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest()
            {
                QueueUrl = name
            };
            List<Amazon.SQS.Model.Message> messages = SQSClient.ReceiveMessage(request).Messages;
            if (messages.Count > 0)
            {
                Amazon.SQS.Model.Message message = messages[0];
                return new Message()
                {
                    ExpirationTime = "none",
                    MessageString = message.Body
                };
            }
            return new Message()
            {
                ExpirationTime = string.Empty,
                MessageString = string.Empty,
                NextVisibleTime = string.Empty,
                DequeueCount = string.Empty
            };
        }

        public override int QueueCount()
        {
            return this.QueueNames().ToList<string>().Count;
        }

        public override int QueueMessageCount(string name)
        {
            ReceiveMessageRequest request = new ReceiveMessageRequest()
            {
                QueueUrl = name
            };
            return SQSClient.ReceiveMessage(request).Messages.Count;
        }

        public override IEnumerable<string> QueueNames()
        {
            ListQueuesRequest request = new ListQueuesRequest()
            {

            };
            return SQSClient.ListQueues(request).QueueUrls;
        }
        public override bool UploadBlockBlobStream(string container, string name, System.IO.Stream file)
        {

            PutObjectRequest request = new PutObjectRequest()
            {
                BucketName = container,
                Key = name,
                InputStream = file
            };
            return Client.PutObject(request).HttpStatusCode == HttpStatusCode.OK;
        }
    }
}
