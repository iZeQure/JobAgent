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
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
        int dwLogonType, int dwLogonProvider, out SafeAccessTokenHandle phToken);

        const int LOGON32_PROVIDER_DEFAULT = 0;
        //This parameter causes LogonUser to create a primary token. 
        const int LOGON32_LOGON_INTERACTIVE = 2;
        // Call LogonUser to obtain a handle to an access token. 
        SafeAccessTokenHandle _safeAccessTokenHandle;

        private readonly string _uncPath;

        public FileAccessProvider()
        {
            _uncPath = EnvironmentProvider.GetUncPath;
        }

        public IEnumerable<string> GetFileCollectionFromContractsShare()
        {
            bool returnValue = LogonUser("Jobagent", @"\\172.17.0.62", "Kode1234!",
            LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
            out _safeAccessTokenHandle);

            WindowsIdentity.RunImpersonated(_safeAccessTokenHandle, () =>
            {                                                   // 
                var directoryFiles = Directory.EnumerateFiles($@"\\{_uncPath}");

                if (!directoryFiles.Any())
                {
                    return Enumerable.Empty<string>();
                }

                return directoryFiles;
            });

            return Enumerable.Empty<string>();
        }
    }
}
