using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace JobAgent.Data.Providers
{
    public class FileAccessProvider
    {
        private readonly string _directory;

        public FileAccessProvider()
        {
            _directory = EnvironmentProvider.GetVirtualDirectory;
        }

        public IEnumerable<FileInfo> GetFileCollectionFromContractsShare()
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo driveInfo in drives)
                {
                    if (driveInfo.Name.Equals(_directory))
                    {
                        DirectoryInfo dirInfo = driveInfo.RootDirectory;

                        DirectoryInfo[] dirInfos = dirInfo.GetDirectories();
                        FileInfo[] files = null;

                        foreach (DirectoryInfo directory in dirInfos)
                        {
                            if (directory.Name.Equals("_contracts"))
                            {
                                files = directory.GetFiles();
                            }
                        }

                        if (files != null)
                        {
                            return files;
                        }
                    }
                }

                return Enumerable.Empty<FileInfo>();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
