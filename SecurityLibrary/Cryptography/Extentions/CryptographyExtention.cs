using ObjectLibrary.Common;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityLibrary.Cryptography.Extentions
{
    public static class CryptographyExtention
    {
        /// <summary>
        /// Hashes the given password of the specified user.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A hashed password.</returns>
        public static string HashPassword(this User value)
        {
            try
            {
                byte[] encodedBytes = Encoding.UTF8.GetBytes(value.Password + value.Salt);
                byte[] computedHashCode = new SHA384Managed().ComputeHash(encodedBytes);

                return Convert.ToBase64String(computedHashCode);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Generates a salt, salt length is determined by the length of the password.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException">Is thrown if password is null or empty.</exception>
        /// <exception cref="ArgumentNullException">Is thrown when the converted array is null.</exception>
        /// <returns>A generated salt.</returns>
        public static string GenerateSalt(this User value)
        {
            try
            {
                if (string.IsNullOrEmpty(value.Password))
                {
                    throw new ArgumentException("Could'n generate salt. Password was undefined.");
                }

                byte[] buffer = new byte[value.Password.Length];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetNonZeroBytes(buffer);
                }

                return Convert.ToBase64String(buffer);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
