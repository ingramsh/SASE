using System;
namespace SASELibrary
{
    interface Account
    {
        System.Collections.Generic.List<string> SASEBlobContainerNames();
        System.Collections.Generic.List<string> SASEBlobInfo(string container, string item);
        System.Collections.Generic.List<string> SASEBlobItemNames(string container);
        System.Collections.Generic.List<string> SASEBlobItems(string container);
        int SASEContainerCount();
        bool SASECreateContainer(string name);
        bool SASECreateQueue(string name);
        string SASEDequeueMessage(string name);
        bool SASEDownloadBlobBlock(string container, string item, string filepath);
        byte[] SASEDownloadBlobBytes(string container, string item);
        System.IO.Stream SASEDownloadBlobStream(string container, string item);
        bool SASEEnqueueMessage(string name, string message);
        System.Collections.Generic.List<string> SASEPeekMessage(string name);
        int SASEQueueCount();
        int SASEQueueMessageCount(string name);
        System.Collections.Generic.List<string> SASEQueueNames();
        bool SASEUploadBlockBlob(string container, string filepath);
        bool SASEUploadBlockBlobBytes(string container, string name, byte[] file);
        bool SASEUploadBlockBlobStream(string container, string name, System.IO.Stream file);
    }
}
