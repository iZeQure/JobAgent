using System.Security.Cryptography;

namespace JobAgentClassLibrary.Security.Cryptography.Hashing.Generators
{
    class SaltGenerator : ISaltGenerator
    {
        public byte[] GenerateSalt(int saltLength = 32)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var rn = new byte[saltLength];
                rng.GetBytes(rn);

                return rn;
            }
        }
    }
}
