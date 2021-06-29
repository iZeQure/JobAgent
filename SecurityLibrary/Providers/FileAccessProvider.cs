using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.Providers
{
    public class FileAccessProvider
    {
        public IEnumerable<FileInfo> GetFileCollectionFromContractsShare(string directoryShare)
        {
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();

                foreach (DriveInfo driveInfo in drives)
                {
                    if (driveInfo.Name.Equals(directoryShare))
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
