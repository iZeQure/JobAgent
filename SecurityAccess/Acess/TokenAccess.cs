using Microsoft.IdentityModel.Tokens;
using Pocos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecurityAccess.Acess
{
    public class TokenAccess : IAccess
    {
        public string GenerateAccess(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = GetClaimsIdentity(user);

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

        public ClaimsIdentity GetClaimsIdentity(User user)
        {
            // Initialize new identity.
            ClaimsIdentity identity = new ClaimsIdentity();

            // Check if the object isn't initialized.
            if (user != null)
            {
                // Check if the obj is correct.
                if (user.Identifier != 0)
                {
                    identity = new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim("UserId", $"{user.Identifier}"),
                                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                                    new Claim(ClaimTypes.Email, user.Email),
                                    new Claim(ClaimTypes.Role, user.ConsultantArea.Name),
                                    new Claim("LocationName", $"{user.Location.Name}")
                                }, $"{user.AccessToken}");
                }
            }

            return identity;
        }
    }
}
