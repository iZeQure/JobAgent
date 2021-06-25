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
    public interface IUserService
    {
        Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation);
        Task<int> RemoveUserAreaAsync(IUser user, int areaId, CancellationToken cancellation);

        string GenerateAccessToken(IUser user);
        ClaimsIdentity GetClaimsIdentity(IUser user);
    }
}
