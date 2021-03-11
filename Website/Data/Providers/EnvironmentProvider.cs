using System;
using System.IO;

namespace JobAgent.Data.Providers
{
    public abstract class EnvironmentProvider
    {
        private const string IP_ADDRESS = "IP_ADDRESS";
        private const string DOMAIN_NAME = "DOMAIN";
        private const string UNC_CONTRACT_SHARE = "UNCSHARE_CONTRACTS";

        public static string GetCustomEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }

        public static string GetIpAddress { get { return Environment.GetEnvironmentVariable(IP_ADDRESS); } }

        public static string GetDomainName { get { return Environment.GetEnvironmentVariable(DOMAIN_NAME); } }

        public static string GetUncPath
        {
            get
            {
                return Environment.GetEnvironmentVariable(IP_ADDRESS) + Environment.GetEnvironmentVariable(UNC_CONTRACT_SHARE);
            }
        }
    }
}
