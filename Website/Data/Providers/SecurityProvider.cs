using Pocos;
using SecurityAccess.Access;
using SecurityAccess.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Providers
{
    public class SecurityProvider
    {
        private readonly Hash _hashInstance;
        private readonly Salt _saltInstance;
        private readonly IAccess _access;

        public SecurityProvider()
        {
            _hashInstance = Hash.Instance;
            _saltInstance = Salt.Instance;
            _access = new TokenAccess();
        }

        public string GenerateAccessToken(User user)
        {
            return _access.GenerateAccess(user);
        }

        public string HashPassword(string password, string salt)
        {
            return _hashInstance.GenerateHashedPassword(password, salt);
        }

        public string GetNewSalt()
        {
            return ConvertSaltToBase64(_saltInstance.GetSalt);
        }

        private static string ConvertSaltToBase64(byte[] salt)
        {
            return Convert.ToBase64String(salt);
        }
    }
}
