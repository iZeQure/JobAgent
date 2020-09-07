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
        public async Task<bool> UploadFileAsync(IFileListEntry fileEntry)
        {
            var path = Path.Combine(@"\\JOB-AGENT\contracts\", fileEntry.Name);
            //var path = Path.Combine(@"C:\Users\Energy Formula\Desktop\Test\", fileEntry.Name);

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
