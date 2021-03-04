using BlazorInputFile;
using JobAgent.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            AddNetorkCredentials();
        }

        public string GetSharedPath { get { return _sharedPath; } }

        public bool CheckFileExists(string fileName)
        {
            Console.WriteLine($"Checking that {fileName} exists...");

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine($"File name was empty");
                    return false;
                }

                foreach (string file in Directory.EnumerateFiles(_sharedPath))
                {
                    string fn = Path.GetFileName(file);

                    Console.WriteLine($"Comparing this file => {fn} with {fileName}");

                    if (fn.Equals(fileName, StringComparison.Ordinal))
                    {
                        Console.WriteLine($"Found a match on => {fn}");

                        return true;
                    }
                }

                Console.WriteLine($"No matches found with => {fileName}");

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
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
            Console.WriteLine($"Attemtps to get {fileName} from directory..");

            if (string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine($"File name wasn't specified.");

                return null;
            }

            try
            {
                foreach (string file in Directory.EnumerateFiles(_sharedPath))
                {
                    string fn = Path.GetFileName(file);

                    if (fn.Equals(fileName))
                    {
                        Console.WriteLine($"Found a match on => {fn}");

                        byte[] bytes = await File.ReadAllBytesAsync(file);

                        return bytes;
                    }
                }

                Console.WriteLine($"No matches found with => {fileName}");

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

        private void AddNetorkCredentials()
        {
            NetworkCredential theNetworkCredential = new NetworkCredential($"jobagent", "Kode1234!");
            CredentialCache theNetcache = new CredentialCache();

            theNetcache.Add(host: _sharedPath, port: 139, "Basic", theNetworkCredential);
        }
    }
}
