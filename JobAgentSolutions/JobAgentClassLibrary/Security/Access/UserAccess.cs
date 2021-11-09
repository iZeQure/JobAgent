using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Settings;
using JobAgentClassLibrary.Security.interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Security.Access
{
    /// <summary>
    /// Represents a Token Access Handler.
    /// </summary>
    public class UserAccess : IAuthenticationAccess
    {
        private readonly ISecuritySettings _securitySettings;
        private readonly IRoleService _roleService;
        private readonly ILocationService _locationService;

        public UserAccess(ISecuritySettings securitySettings, ILocationService locationService, IRoleService roleService)
        {
            _securitySettings = securitySettings;
            _locationService = locationService;
            _roleService = roleService;
        }

        /// <summary>
        /// Generates a new access token, to the given <see cref="User"/>.
        /// </summary>
        /// <param name="user">A user to grant access.</param>
        /// <returns>An authenticated access token.</returns>
        public async Task<string> GenerateAccessTokenAsync(IAuthUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = await GetClaimsIdentityAsync(user);

            var token = tokenHandler.CreateJwtSecurityToken(
                issuer: "jobagent.nu",
                audience: "jobagent.nu",
                subject: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(5)),
                signingCredentials:
                new SigningCredentials(
                    new SymmetricSecurityKey(
                        Encoding.Default.GetBytes(_securitySettings.JwtSecurityKey)),
                        SecurityAlgorithms.HmacSha256Signature));

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Acquires a new claims identify, authorized as the given user. Will never return null.
        /// </summary>
        /// <param name="user">A user to authorize.</param>
        /// <returns>An initialized context of <see cref="ClaimsIdentity"/> authorized by the given <see cref="IUser"/> else empty.</returns>
        public async Task<ClaimsIdentity> GetClaimsIdentityAsync(IAuthUser user)
        {
            // Initialize new identity.
            ClaimsIdentity identity = new();

            // Check if the object isn't initialized.
            if (ObjectIsNotNull(user) && user is AuthUser authUser)
            {
                // Check if the obj is correct.
                if (HasValidIdentity(authUser.Id))
                {
                    var generatedConsultantAreaClaims = GenerateConsultantAreaClaimsEntities(authUser.ConsultantAreas, "ConsultantArea");
                    var userRole = await _roleService.GetRoleByIdAsync(authUser.RoleId);
                    var userLocation = await _locationService.GetByIdAsync(authUser.LocationId);
                    
                    List<Claim> identityClaims = new()
                    {
                        new Claim(ClaimTypes.NameIdentifier, authUser.Id.ToString()),
                        new Claim(ClaimTypes.Authentication, bool.TrueString),
                        new Claim(ClaimTypes.Name, authUser.FullName),
                        new Claim(ClaimTypes.Email, authUser.Email),
                        new Claim(ClaimTypes.Role, userRole.Name),
                        new Claim("Location", $"{userLocation.Name}")
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
        private static IEnumerable<Claim> GenerateConsultantAreaClaimsEntities(List<IArea> areas, string claimsType)
        {
            var tempClaims = new List<Claim>();
            for (int i = 0; i < areas.Count; i++)
            {
                tempClaims.Add(
                    new Claim(claimsType, areas[i].Name));
            }
            return tempClaims;
        }
    }
}
