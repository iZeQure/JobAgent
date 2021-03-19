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
            _sharedPath = EnvironmentProvider.GetVirtualDirectory;
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
                    var files = new FileAccessProvider().GetFileCollectionFromContractsShare();

                    if (!files.Any())
                    {
                        throw new FileNotFoundException();
                    }

                    for (int i = 0; i < files.Count(); i++)
                    {
                        var extendedFileName = Path.GetFileName(files.ElementAt(i).FullName);

                        if (extendedFileName.Equals(fileName, StringComparison.Ordinal))
                        {
                            return true;
                        }
                    }

                    Debug.WriteLine($"No matches found with => {fileName}");

                    return false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to get File Collection from Contracts Share : {ex.Message}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Exception : {ex.Message}");
                throw;
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
            Debug.WriteLine($"Attemtps to get {fileName} from directory..");

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.WriteLine($"File name wasn't specified.");

                return null;
            }

            try
            {
                foreach (FileInfo file in new FileAccessProvider().GetFileCollectionFromContractsShare())
                {
                    if (file.Name.Equals(fileName))
                    {
                        Debug.WriteLine($"Found a match on => {file.Name}");

                        byte[] bytes = await File.ReadAllBytesAsync(file.FullName);

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
