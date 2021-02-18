using JobAgent.Data.Security;
using JobAgent.Models;
using Pocos;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class SecurityService
    {
        /// <summary>
        /// Generate new salt, on user account creation.
        /// </summary>
        /// <returns>A salt in base64 format.</returns>
        public string GetNewSaltAsync()
        {
            return Convert.ToBase64String(Salt.Instance.GetSalt);
        }

        /// <summary>
        /// Hash password, on user account creation.
        /// </summary>
        /// <param name="password">Used to identify the password to hash.</param>
        /// <param name="salt">Used to secure the password hash.</param>
        /// <returns>A hashed password.</returns>
        public string HashPasswordAsync(string password, string salt)
        {
            return Hash.Instance.GenerateHashedPassword(password, salt);
        }

        public RefreshTokenModel GenerateRefreshToken()
        {
            RefreshTokenModel refreshToken = new RefreshTokenModel();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        public string GenerateAccessToken(User user)
        {
            MyAuthStateProvider myAuth = new MyAuthStateProvider();

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var claims = myAuth.GetClaimsIdentity(user);

            //var token = tokenHandler.CreateJwtSecurityToken(
            //    issuer: "jobagent.zbc.dk",
            //    audience: "jobagent.zbc.dk",
            //    subject: claims,
            //    notBefore: DateTime.UtcNow,
            //    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(5)),
            //    signingCredentials:
            //    new SigningCredentials(
            //        new SymmetricSecurityKey(
            //            Encoding.Default.GetBytes("vK+DQL*Gpwks[ZaBxBG+x-bQahAB3HmtlnAKIAf5Wn5jogM;bcQms-NtobauCXZo")),
            //            SecurityAlgorithms.HmacSha256Signature));

            //return tokenHandler.WriteToken(token);

            return new Guid().ToString();
        }
    }
}
