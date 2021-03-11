using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JobAgent.Data.Providers
{
    public class FileAccessProvider
    {
        private readonly string _uncPath;

        public FileAccessProvider()
        {
            _uncPath = EnvironmentProvider.GetUncPath;
        }

        public IEnumerable<string> GetFileCollectionFromContractsShare()
        {
            var directoryFiles = Directory.EnumerateFiles($@"\\{_uncPath}", ".pdf", SearchOption.AllDirectories);

            if (!directoryFiles.Any())
            {
                return Enumerable.Empty<string>();
            }

            return directoryFiles;
        }
    }
}
