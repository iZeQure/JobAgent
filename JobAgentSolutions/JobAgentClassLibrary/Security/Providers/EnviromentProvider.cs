using System;

namespace JobAgentClassLibrary.Security.Providers
{
    public abstract class EnvironmentProvider
    {
        private const string IP_ADDRESS = "IP_ADDRESS";
        private const string DOMAIN_NAME = "DOMAIN";
        private const string UNC_CONTRACT_SHARE = "UNCSHARE_CONTRACTS";
        private const string VIRTUAL_DIRECTORY = "VIRTUAL_DIRECTORY";

        public static string GetCustomEnvironmentVariable(string variable)
        {
            return GetEnvironmentVariable(variable);
        }

        public static string GetIpAddress { get { return GetEnvironmentVariable(IP_ADDRESS); } }

        public static string GetDomainName { get { return GetEnvironmentVariable(DOMAIN_NAME); } }

        public static string GetVirtualDirectory { get { return GetEnvironmentVariable(VIRTUAL_DIRECTORY); } }

        public static string GetUncPath
        {
            get
            {
                return GetEnvironmentVariable(IP_ADDRESS) + GetEnvironmentVariable(UNC_CONTRACT_SHARE);
            }
        }

        private static string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}
