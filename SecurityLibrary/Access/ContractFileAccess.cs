using SecurityLibrary.Interfaces;
using SecurityLibrary.Providers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecurityLibrary.Access
{
    public class ContractFileAccess : IFileAccess
    {
        private readonly FileAccessProvider _fileAccessProvider;

        public ContractFileAccess(FileAccessProvider fileAccessProvider)
        {
            _fileAccessProvider = fileAccessProvider;
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
                    var files = _fileAccessProvider.GetFileCollectionFromContractsShare(EnvironmentProvider.GetVirtualDirectory);

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

        public async Task<byte[]> GetFileFromDirectoryAsync(string fileName, CancellationToken cancellation)
        {
            Debug.WriteLine($"Attemtps to get {fileName} from directory..");

            if (string.IsNullOrEmpty(fileName))
            {
                Debug.WriteLine($"File name wasn't specified.");

                return null;
            }

            try
            {
                foreach (FileInfo file in _fileAccessProvider.GetFileCollectionFromContractsShare(EnvironmentProvider.GetVirtualDirectory))
                {
                    if (file.Name.Equals(fileName))
                    {
                        Debug.WriteLine($"Found a match on => {file.Name}");

                        byte[] bytes = await File.ReadAllBytesAsync(file.FullName, cancellation);

                        return await Task.FromResult(bytes);
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

        public async Task<string> UploadFIleAsync(byte[] fileBytes, CancellationToken cancellation)
        {
            string fileName = $"{Guid.NewGuid()}.pdf";

            try
            {
                var contractPath = Path.Combine(EnvironmentProvider.GetVirtualDirectory, fileName);

                await File.WriteAllBytesAsync(contractPath, fileBytes, cancellation);

                return await Task.FromResult(fileName);
            }
            catch (Exception)
            {
                return await Task.FromResult(string.Empty);
            }
        }
    }
}
