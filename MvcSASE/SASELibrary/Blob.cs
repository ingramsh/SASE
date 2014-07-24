using System;
namespace SASELibrary
{
    abstract class Blob
    {
        System.Collections.Generic.List<string> BlobInfo(string container, string item);
        bool CreateContainer(string name);
        void DownloadBlobItem(string container, string item, string filepath);
        System.IO.Stream DownloadBlobStream(string container, string item);
        byte[] GetBlobBytes(string container, string item);
        System.Collections.Generic.List<string> GetBlobItemNames(string container);
        System.Collections.Generic.List<string> GetContainerNames();
        void UploadBlockBlob(string container, string name, string filepath);
        void UploadBlockBlobBytes(string container, string name, byte[] file);
        void UploadBlockBlobStream(string container, string name, System.IO.Stream file);
    }
}
