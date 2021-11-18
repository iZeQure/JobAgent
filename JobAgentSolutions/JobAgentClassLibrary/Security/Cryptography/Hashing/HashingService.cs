using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Factory;
using JobAgentClassLibrary.Security.Cryptography.Hashing.Generators;
using System;
using System.Security.Cryptography;
using System.Text;

namespace JobAgentClassLibrary.Security.Cryptography.Hashing
{
    public class HashingService : ICryptographyService
    {
        private readonly UserEntityFactory _userFactory;

        public HashingService(UserEntityFactory userFactory)
        {
            _userFactory = userFactory;
        }

        /// <summary>
        /// Creates a hashed user.
        /// This method is used for when a new user is created.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>A <see cref="IHashedUser"/> with the hashed password and generated salt.</returns>
        public IHashedUser CreateHashedUser(string password)
        {
            string salt = Convert.ToBase64String(SaltGeneratorFactory.GetSaltGenerator().GenerateSalt());

            byte[] encodedBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] computedHashCode = new SHA384Managed().ComputeHash(encodedBytes);

            password = Convert.ToBase64String(computedHashCode);
            IHashedUser hashedUser = (IHashedUser)_userFactory.CreateEntity(nameof(HashedUser), password, salt);

            return hashedUser;
        }

        public string HashUserPasswordWithSalt(string password, string salt)
        {
            byte[] encodedBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] computedHashCode = new SHA384Managed().ComputeHash(encodedBytes);

            return Convert.ToBase64String(computedHashCode);
        }
    }
}
