using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Data.Security
{
    public sealed class Hash
    {
        private static Hash instance = null;
        private static readonly object hashLock = new object();

        private Hash() { }

        public static Hash Instance
        {
            get
            {
                lock (hashLock)
                {
                    if (instance == null)
                    {
                        return new Hash();
                    }

                    return instance;
                }
            }
        }

        public string GenerateHashedPassword(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA384Managed sHA384ManagedString = new SHA384Managed();
            byte[] hash = sHA384ManagedString.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
