using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SASELibrary
{
    public class AWSBlob: Blob
    {
        public override List<string> BlobInfo(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override bool CreateContainer(string name)
        {
            throw new NotImplementedException();
        }

        public override void DownloadBlobItem(string container, string item, string filepath)
        {
            throw new NotImplementedException();
        }

        public override System.IO.Stream DownloadBlobStream(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetBlobBytes(string container, string item)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetBlobItemNames(string container)
        {
            throw new NotImplementedException();
        }

        public override List<string> GetContainerNames()
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
