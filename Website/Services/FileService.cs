using BlazorInputFile;
using JobAgent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class FileService : IFileService
    {
        private readonly string _sharedPath;

        public FileService()
        {
            _sharedPath = Environment.GetEnvironmentVariable("SHARED_CONTRACT_PATH");
        }

        public string GetSharedPath { get { return _sharedPath; } }

        public bool CheckFileExists(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName)) return false;

                foreach (string file in Directory.EnumerateFiles(_sharedPath))
                {
                    if (file.Equals(fileName))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string EncodeFileToBase64(byte[] fileBytes)
        {
            if (fileBytes == null)
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToBase64String(fileBytes);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<byte[]> GetFileFromDirectoryAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            try
            {
                foreach (string file in Directory.EnumerateFiles(_sharedPath))
                {
                    if (file.Equals(fileName))
                    {
                        byte[] bytes = await File.ReadAllBytesAsync(file);

                        return bytes;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UploadFileAsync(IFileListEntry fileEntry)
        {
            var path = Path.Combine(_sharedPath, fileEntry.Name);

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
