using JobAgent.Data.Providers;
using JobAgent.Services.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class FileService : IFileService
    {
        private readonly string _sharedPath;

        public FileService()
        {
            _sharedPath = EnvironmentProvider.GetUncPath;
        }

        public bool CheckFileExists(string fileName)
        {
            Debug.WriteLine($"Checking that {fileName} exists...");

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Debug.WriteLine($"File name was empty");
                    return false;
                }

                try
                {
                    var fileNames = new FileAccessProvider().GetFileCollectionFromContractsShare();

                    if (!fileNames.Any())
                    {
                        return false;
                    }

                    for (int i = 0; i < fileNames.Count(); i++)
                    {
                        var extendedFileName = Path.GetFileName(fileNames.ElementAt(i));

                        if (extendedFileName.Equals(fileName, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }

                    Debug.WriteLine($"No matches found with => {fileName}");

                    return false;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception) { throw; }
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
            Debug.WriteLine($"Attemtps to get {fileName} from directory..");

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.WriteLine($"File name wasn't specified.");

                return null;
            }

            try
            {
                foreach (string file in Directory.EnumerateFiles(_sharedPath))
                {
                    string fn = Path.GetFileName(file);

                    if (fn.Equals(fileName))
                    {
                        Debug.WriteLine($"Found a match on => {fn}");

                        byte[] bytes = await File.ReadAllBytesAsync(file);

                        return bytes;
                    }
                }

                Debug.WriteLine($"No matches found with => {fileName}");

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadFileAsync(byte[] file)
        {
            string fileName = $"{Guid.NewGuid()}.pdf";

            try
            {
                var contractPath = Path.Combine(_sharedPath, fileName);

                await File.WriteAllBytesAsync(contractPath, file);

                return fileName;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
