using ObjectLibrary.Common;
using SqlDataAccessLibrary.Repositories;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    /// <summary>
    /// Represents a User specific interface.
    /// </summary>
    public interface IUserService : IBaseService<IUser>
    {
        Task<IUser> LoginAsync(string email, string password, CancellationToken cancellation);

        Task<IUser> GetUserByAccessTokenAsync(string token, CancellationToken cancellation);

        Task<IUser> GetUserByEmailAsync(string email, CancellationToken cancellation);

        Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation);

        Task<int> RemoveUserAreaAsync(IUser user, int areaId, CancellationToken cancellation);

        Task<int> UpdateUserPasswordAsync(IUser user, CancellationToken cancellation);        

        Task<bool> ValidateUserExistsByEmail(string userEmail, CancellationToken cancellation);

        /// <summary>
        /// Generates an authenticated access token to the <see cref="IUser"/>.
        /// </summary>
        /// <param name="user">A user to authenticate.</param>
        /// <returns>A generated token if the user is found; else <see cref="string.Empty"/>.</returns>
        string GenerateAccessToken(IUser user);

        ClaimsIdentity GetClaimsIdentity(IUser user);
    }
}
