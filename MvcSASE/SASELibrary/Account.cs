using System;
namespace SASELibrary
{
    public abstract class Account
    {
        public abstract System.Collections.Generic.List<string> SASEBlobContainerNames();
        public abstract System.Collections.Generic.List<string> SASEBlobInfo(string container, string item);
        public abstract System.Collections.Generic.List<string> SASEBlobItemNames(string container);
        public abstract System.Collections.Generic.List<string> SASEBlobItems(string container);
        public abstract int SASEContainerCount();
        public abstract bool SASECreateContainer(string name);
        public abstract bool SASECreateQueue(string name);
        public abstract string SASEDequeueMessage(string name);
        public abstract bool SASEDownloadBlobBlock(string container, string item, string filepath);
        public abstract byte[] SASEDownloadBlobBytes(string container, string item);
        public abstract System.IO.Stream SASEDownloadBlobStream(string container, string item);
        public abstract bool SASEEnqueueMessage(string name, string message);
        public abstract System.Collections.Generic.List<string> SASEPeekMessage(string name);
        public abstract int SASEQueueCount();
        public abstract int SASEQueueMessageCount(string name);
        public abstract System.Collections.Generic.List<string> SASEQueueNames();
        public abstract bool SASEUploadBlockBlob(string container, string filepath);
        public abstract bool SASEUploadBlockBlobBytes(string container, string name, byte[] file);
        public abstract bool SASEUploadBlockBlobStream(string container, string name, System.IO.Stream file);
    }
}
