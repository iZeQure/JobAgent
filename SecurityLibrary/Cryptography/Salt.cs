using System.Security.Cryptography;

namespace SecurityLibrary.Cryptography
{
    public sealed class Salt
    {
        private static readonly Salt _instance = null;
        private static readonly object _saltLock = new object();

        private static readonly int _saltLengthLimit = 64;
        private static byte[] _salt;

        private Salt() { }

        public static Salt Instance
        {
            get
            {
                lock (_saltLock)
                {
                    if (_instance == null)
                    {
                        return new Salt();
                    }

                    return _instance;
                }
            }
        }

        public byte[] GetSalt
        {
            get
            {
                return GenerateSalt(_saltLengthLimit);
            }
        }

        private static byte[] GenerateSalt(int saltLengthLimit)
        {
            _salt = new byte[saltLengthLimit];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(_salt);
            }

            return _salt;
        }
    }
}
