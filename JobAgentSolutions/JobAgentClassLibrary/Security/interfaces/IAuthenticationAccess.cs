using JobAgentClassLibrary.Common.Users.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Security.interfaces
{
    public interface IAuthenticationAccess
    {
        /// <summary>
        /// Generates a new access token, to the given <see cref="IAuthUser"/>.
        /// </summary>
        /// <param name="user">A user to grant access.</param>
        /// <returns>An authenticated access token.</returns>
        Task<string> GenerateAccessTokenAsync(IAuthUser user);

        /// <summary>
        /// Acquires a new claims identify, authorized as the given user. Will never return null.
        /// </summary>
        /// <param name="user">A user to authorize.</param>
        /// <returns>An initialized context of <see cref="ClaimsIdentity"/> authorized by the given <see cref="IAuthUser"/> else empty.</returns>
        Task<ClaimsIdentity> GetClaimsIdentityAsync(IAuthUser user);
    }
}
