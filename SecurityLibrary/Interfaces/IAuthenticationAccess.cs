using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ObjectLibrary.Common;

namespace SecurityLibrary.Interfaces
{
    public interface IAuthenticationAccess
    {
        /// <summary>
        /// Generates a new access token, to the given <see cref="IUser"/>.
        /// </summary>
        /// <param name="user">A user to grant access.</param>
        /// <returns>An authenticated access token.</returns>
        string GenerateAccessToken(IUser user);

        /// <summary>
        /// Acquires a new claims identify, authorized as the given user. Will never return null.
        /// </summary>
        /// <param name="user">A user to authorize.</param>
        /// <returns>An initialized context of <see cref="ClaimsIdentity"/> authorized by the given <see cref="IUser"/> else empty.</returns>
        ClaimsIdentity GetClaimsIdentity(IUser user);
    }
}
