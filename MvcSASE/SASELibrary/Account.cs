using System;
using System.Collections.Generic;
namespace SASELibrary
{
    public abstract class Account
    {
        public abstract IEnumerable<string> BlobContainerNames();
        public abstract IEnumerable<string> BlobInfo(string container, string item);
        public abstract IEnumerable<string> BlobItemNames(string container);
        public abstract IEnumerable<string> BlobItems(string container);
        public abstract int ContainerCount();
        public abstract bool CreateContainer(string name);
        public abstract bool CreateQueue(string name);
        public abstract string DequeueMessage(string name);
        public abstract bool DownloadBlobBlock(string container, string item, string filepath);
        public abstract byte[] DownloadBlobBytes(string container, string item);
        public abstract System.IO.Stream DownloadBlobStream(string container, string item);
        public abstract bool EnqueueMessage(string name, string message);
        public abstract IEnumerable<string> PeekMessage(string name);
        public abstract int QueueCount();
        public abstract int QueueMessageCount(string name);
        public abstract IEnumerable<string> QueueNames();
        public abstract bool UploadBlockBlob(string container, string filepath);
        public abstract bool UploadBlockBlobBytes(string container, string name, byte[] file);
        public abstract bool UploadBlockBlobStream(string container, string name, System.IO.Stream file);
    }
}
