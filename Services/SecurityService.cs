using JobAgent.Data;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Data.Security;
using JobAgent.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        public Task<string> GenerateAccessTokenAsync(User user)
        {
            return Task.FromResult(GenerateAccessToken(user));
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

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = myAuth.GetClaimsIdentity(user);

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "jobagent.zbc.dk",
                audience: "jobagent.zbc.dk",
                subject: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(5)),
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.Default.GetBytes("vK+DQL*Gpwks[ZaBxBG+x-bQahAB3HmtlnAKIAf5Wn5jogM;bcQms-NtobauCXZo")),
                        SecurityAlgorithms.HmacSha256Signature));

            return tokenHandler.WriteToken(token);
        }
    }
}
