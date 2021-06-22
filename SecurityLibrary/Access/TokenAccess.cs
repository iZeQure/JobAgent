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
        private readonly IConfigurationSettings _configurationSettings;

        public TokenAccess(IConfigurationSettings configurationSettings)
        {
            _configurationSettings = configurationSettings;
        }

        /// <summary>
        /// Generates a new access token, to the given <see cref="User"/>.
        /// </summary>
        /// <param name="user">A user to grant access.</param>
        /// <returns>An authenticated access token.</returns>
        public string GenerateAccessToken(User user)
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
                        Encoding.Default.GetBytes(_configurationSettings.JwtSecurityKey)),
                        SecurityAlgorithms.HmacSha256Signature));

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Acquires a new claims identify, authorized as the given user. Will never return null.
        /// </summary>
        /// <param name="user">A user to authorize.</param>
        /// <returns>An initialized context of <see cref="ClaimsIdentity"/> authorized by the given <see cref="User"/> else empty.</returns>
        public ClaimsIdentity GetClaimsIdentity(User user)
        {
            // Initialize new identity.
            ClaimsIdentity identity = new ();

            // Check if the object isn't initialized.
            if (ObjectIsNotNull(user))
            {
                // Check if the obj is correct.
                if (HasValidIdentity(user.Id))
                {
                    var generatedConsultantAreaClaims = GenerateConsultantAreaClaimsEntities(user.GetConsultantAreas, "ConsultantArea");
                    List<Claim> identityClaims = new()
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Authentication, bool.TrueString),
                        new Claim(ClaimTypes.Name, user.GetFullName),
                        new Claim(ClaimTypes.Email, user.GetEmail),
                        new Claim(ClaimTypes.Role, user.GetRole.Name),
                        new Claim("Location", $"{user.GetLocation.Name}")
                    };
                    identityClaims.AddRange(generatedConsultantAreaClaims);
                    //identity = new ClaimsIdentity(identityClaims, user.AccessToken);
                    identity = new ClaimsIdentity(
                        claims: identityClaims,
                        authenticationType: "Password");
                }

            }
            return identity;
        }

        /// <summary>
        /// Checks whether an object is null.
        /// </summary>
        /// <param name="obj">An object to validate.</param>
        /// <returns>true if not null, otherwise false.</returns>
        private static bool ObjectIsNotNull(object obj)
        {
            return obj != null;
        }

        private static bool HasValidIdentity(int id)
        {
            return id != 0;
        }

        /// <summary>
        /// Generates a specified amount of claims for the given lists of entities and claims type.
        /// </summary>
        /// <param name="entities">A Collection of consultant areas.</param>
        /// <param name="claimsType">The name of the claims type.</param>
        /// <returns></returns>
        private static IEnumerable<Claim> GenerateConsultantAreaClaimsEntities(IEnumerable<Area> areas, string claimsType)
        {
            var tempClaims = new List<Claim>();
            for (int i = 0; i < areas.Count(); i++)
            {
                tempClaims.Add(
                    new Claim(claimsType, areas.ElementAt(i).Name));
            }
            return tempClaims;
        }
    }
}
