using System;
using System.Collections.Generic;
using System.Web;
namespace SASELibrary
{
    public abstract class Blob
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public abstract IEnumerable<string> BlobInfo(string container, string item);
        public abstract bool CreateContainer(string name);
        public abstract void DownloadBlobItem(string container, string item, string filepath);
        public abstract System.IO.Stream DownloadBlobStream(string container, string item);
        public abstract byte[] GetBlobBytes(string container, string item);
        public abstract IEnumerable<string> GetBlobItemNames(string container);
        public abstract IEnumerable<string> GetContainerNames();
        public abstract void UploadBlockBlob(string container, string name, string filepath);
        public abstract void UploadBlockBlobBytes(string container, string name, byte[] file);
        public abstract void UploadBlockBlobStream(string container, string name, System.IO.Stream file);

        protected string DecodeURL(string url)
        {
            return HttpUtility.UrlDecode(url);
        }
    }
}
