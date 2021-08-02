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
        public static void HashPassword(this IUser value)
        {
            try
            {
                byte[] encodedBytes = Encoding.UTF8.GetBytes(value.GetPassword + value.GetSalt);
                byte[] computedHashCode = new SHA384Managed().ComputeHash(encodedBytes);

                value.SetPassword(Convert.ToBase64String(computedHashCode));
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
        public static void GenerateSalt(this IUser value)
        {
            try
            {
                if (string.IsNullOrEmpty(value.GetPassword))
                {
                    throw new ArgumentException("Could'n generate salt. Password was undefined.");
                }

                byte[] buffer = new byte[256];

                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetNonZeroBytes(buffer);
                }

                value.SetSalt(Convert.ToBase64String(buffer));
            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
