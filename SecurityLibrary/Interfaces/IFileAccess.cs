using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecurityLibrary.Interfaces
{
    public interface IFileAccess
    {
        Task<string> UploadFIleAsync(byte[] fileBytes, CancellationToken cancellation);
        Task<byte[]> GetFileFromDirectoryAsync(string fileName, CancellationToken cancellation);
        bool CheckFileExists(string fileName);
        string EncodeFileToBase64(byte[] fileBytes);
    }
}
