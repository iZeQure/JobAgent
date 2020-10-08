using BlazorInputFile;
using JobAgent.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class FileService : IFileUpload
    {
        private readonly string _sharedPath = Environment.GetEnvironmentVariable("SHARED_PATH");

        public string GetSharedPath { get { return _sharedPath; } }

        public async Task<bool> UploadFileAsync(IFileListEntry fileEntry)
        {
            var path = Path.Combine(@"\\Jobagent\contracts\", fileEntry.Name);

            try
            {
                var memoryStream = new MemoryStream();

                await fileEntry.Data.CopyToAsync(memoryStream);

                using FileStream file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                memoryStream.WriteTo(file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
    }
}
