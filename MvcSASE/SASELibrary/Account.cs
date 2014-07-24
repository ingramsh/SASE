using System;
namespace SASELibrary
{
    public abstract class Account
    {
        public abstract System.Collections.Generic.List<string> BlobContainerNames();
        public abstract System.Collections.Generic.List<string> BlobInfo(string container, string item);
        public abstract System.Collections.Generic.List<string> BlobItemNames(string container);
        public abstract System.Collections.Generic.List<string> BlobItems(string container);
        public abstract int ContainerCount();
        public abstract bool CreateContainer(string name);
        public abstract bool CreateQueue(string name);
        public abstract string DequeueMessage(string name);
        public abstract bool DownloadBlobBlock(string container, string item, string filepath);
        public abstract byte[] DownloadBlobBytes(string container, string item);
        public abstract System.IO.Stream DownloadBlobStream(string container, string item);
        public abstract bool EnqueueMessage(string name, string message);
        public abstract System.Collections.Generic.List<string> PeekMessage(string name);
        public abstract int QueueCount();
        public abstract int QueueMessageCount(string name);
        public abstract System.Collections.Generic.List<string> QueueNames();
        public abstract bool UploadBlockBlob(string container, string filepath);
        public abstract bool UploadBlockBlobBytes(string container, string name, byte[] file);
        public abstract bool UploadBlockBlobStream(string container, string name, System.IO.Stream file);
    }
}
