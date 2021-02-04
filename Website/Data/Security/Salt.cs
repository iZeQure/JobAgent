using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace JobAgent.Data.Security
{
    public sealed class Salt
    {
        private static readonly Salt instance = null;
        private static readonly object singletonLock = new object();

        private static readonly int saltLengthLimit = 64;
        private static byte[] salt;

        private Salt() { }

        public static Salt Instance
        {
            get
            {
                lock (singletonLock)
                {
                    if (instance == null)
                    {
                        return new Salt();
                    }

                    return instance;
                }
            }
        }

        [DefaultValue(64)]
        public int SaltLengthLimit
        {
            get
            {
                return saltLengthLimit;
            }
        }

        public byte[] GetSalt
        {
            get
            {
                return GenerateSalt();
            }
        }

        private byte[] GenerateSalt()
        {
            return GenerateSalt(SaltLengthLimit);
        }

        private static byte[] GenerateSalt(int saltLengthLimit)
        {
            salt = new byte[saltLengthLimit];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
