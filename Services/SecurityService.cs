using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class SecurityService
    {
        /// <summary>
        /// Generate new salt, on user account creation.
        /// </summary>
        /// <returns>A new salt.</returns>
        public Task<string> GetNewSaltAsync()
        {
            return Task.FromResult(Convert.ToBase64String(Salt.Instance.GetSalt));
        }

        /// <summary>
        /// Hash password, on user account creation.
        /// </summary>
        /// <param name="password">Used to identify the password to hash.</param>
        /// <param name="salt">Used to secure the password hash.</param>
        /// <returns>A hashed password.</returns>
        public Task<string> HashPasswordAsync(string password, string salt)
        {
            return Task.FromResult(Hash.Instance.GenerateHashedPassword(password, salt));
        }

        public Task<string> GenerateAccessTokenAsync(int id)
        {
            IUserRepository userRepository = new UserRepository();

            return Task.FromResult(userRepository.GenerateAccessToken(id));
        }
    }
}
