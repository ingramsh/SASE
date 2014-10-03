using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace SASELibrary
{
    public class AwsAccountService : AccountService
    {
        private IAmazonS3 _client;
        private IAmazonSQS _sqsClient;

        private IAmazonS3 Client
        {
            get { return _client ?? (_client = new AmazonS3Client(Creds, RegionEndpoint.USEast1)); }
        }

        private IAmazonSQS SqsClient
        {
            get { return _sqsClient ?? (_sqsClient = new AmazonSQSClient(Creds, RegionEndpoint.USWest2)); }
        }

        private BasicAWSCredentials Creds
        {
            get { return new BasicAWSCredentials(storageAccount, storageKey); }
        }

        private IEnumerable<S3Bucket> Buckets
        {
            get { return Client.ListBuckets().Buckets; }
        }

        public override IEnumerable<string> BlobContainerNames()
        {
            return Buckets.Select(b => b.BucketName);
        }

        public override BlobInfo BlobInfo(string container, string item)
        {
            var request = new GetObjectRequest
            {
                BucketName = container,
                Key = item
            };
            GetObjectResponse response = Client.GetObject(request);
            var blobinfo = new BlobInfo
            {
                BlobLocation = response.Key,
                LastModified = response.LastModified.ToString(CultureInfo.InvariantCulture),
                BlobType = "Amazon Blob",
                Length = response.ContentLength.ToString(CultureInfo.InvariantCulture)
            };
            return blobinfo;
        }

        public override IEnumerable<string> BlobItemNames(string container)
        {
            var request = new ListObjectsRequest
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
            var request = new PutBucketRequest
            {
                BucketName = name
            };
            return Client.PutBucket(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override bool CreateQueue(string name)
        {
            var request = new CreateQueueRequest
            {
                QueueName = name
            };
            return SqsClient.CreateQueue(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override string DequeueMessage(string name)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = name
            };

            Amazon.SQS.Model.Message message = SqsClient.ReceiveMessage(request).Messages[0];
            var deleterequest = new DeleteMessageRequest
            {
                QueueUrl = name,
                ReceiptHandle = message.ReceiptHandle
            };
            SqsClient.DeleteMessage(deleterequest);
            return message.Body;
        }

        public override byte[] DownloadBlobBytes(string container, string item)
        {
            GetObjectResponse response = GetDownloadResponse(container, item);
            using (var reader = new BinaryReader(response.ResponseStream))
            {
                var byteArray = reader.ReadBytes((int)response.ContentLength);
                return byteArray;
            }

        }

        public override Stream DownloadBlobStream(string container, string item)
        {
            return GetDownloadResponse(container, item).ResponseStream;
        }

        private GetObjectResponse GetDownloadResponse(string container, string item)
        {
            var request = new GetObjectRequest
            {
                BucketName = container,
                Key = item
            };
            return Client.GetObject(request);
        }

        public override bool EnqueueMessage(string name, string message)
        {
            var request = new SendMessageRequest
            {
                MessageBody = message,
                QueueUrl = name
            };
            return SqsClient.SendMessage(request).HttpStatusCode == HttpStatusCode.OK;
        }

        public override Message PeekMessage(string name)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = name
            };
            List<Amazon.SQS.Model.Message> messages = SqsClient.ReceiveMessage(request).Messages;
            if (messages.Count > 0)
            {
                Amazon.SQS.Model.Message message = messages[0];
                return new Message
                {
                    ExpirationTime = "none",
                    MessageString = message.Body
                };
            }
            return new Message
            {
                ExpirationTime = string.Empty,
                MessageString = string.Empty,
                NextVisibleTime = string.Empty,
                DequeueCount = string.Empty
            };
        }

        public override int QueueCount()
        {
            return QueueNames().ToList().Count;
        }

        public override int QueueMessageCount(string name)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = name
            };
            return SqsClient.ReceiveMessage(request).Messages.Count;
        }

        public override IEnumerable<string> QueueNames()
        {
            var request = new ListQueuesRequest();
            return SqsClient.ListQueues(request).QueueUrls;
        }

        public override bool UploadBlockBlobStream(string container, string name, Stream file)
        {
            var request = new PutObjectRequest
            {
                BucketName = container,
                Key = name,
                InputStream = file
            };
            return Client.PutObject(request).HttpStatusCode == HttpStatusCode.OK;
        }
    }
}