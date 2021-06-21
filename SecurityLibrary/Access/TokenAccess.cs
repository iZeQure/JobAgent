using Microsoft.IdentityModel.Tokens;
using ObjectLibrary.Common;
using SecurityLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SecurityLibrary.Access
{
    public class TokenAccess : IAccess
    {
        //private readonly IConfiguration 

        public TokenAccess()
        {
            
        }

        public string GenerateAccess(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = GetClaimsIdentity(user);

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "jobagent.nu",
                audience: "jobagent.nu",
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
            ClaimsIdentity identity = new ();

            // Check if the object isn't initialized.
            if (user != null)
            {
                // Check if the obj is correct.

                List<Claim> identityClaims = new()
                {
                    new Claim("UserId", $"{user.Id}"),
                    new Claim(ClaimTypes.Name, $"{user.GetFullName}"),
                    new Claim("Email", $"{user.GetEmail}"),
                    new Claim("LocationName", $"{user.GetLocation.Name}")
                };
                List<Claim> areaclaims = new();

                for (int i = 0; i < user.GetConsultantAreas.Count(); i++)
                {
                    areaclaims.Add(new Claim("ConsultingArea", $"{user.GetConsultantAreas.ElementAt(i)}"));
                }

                identityClaims.AddRange(areaclaims);

                identity = new ClaimsIdentity(identityClaims, user.AccessToken);

            }
            return identity;
        }
    }
}
