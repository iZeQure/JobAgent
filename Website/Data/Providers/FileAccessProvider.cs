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

        public IEnumerable<string> GetFileCollectionFromContractsShare()
        {
            var directoryFiles = Directory.EnumerateFiles(_directory);

            if (!directoryFiles.Any())
            {
                return Enumerable.Empty<string>();
            }

            return directoryFiles;
        }
    }
}
