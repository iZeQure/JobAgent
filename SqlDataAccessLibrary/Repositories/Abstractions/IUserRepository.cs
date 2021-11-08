using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<IUser>
    {
        Task<bool> AuthenticateUserLoginAsync(IUser user, CancellationToken cancellation);

        Task<IUser> GetUserByAccessTokenAsync(string accessToken, CancellationToken cancellation);

        Task<IUser> GetByEmailAsync(string email, CancellationToken cancellation);

        Task<bool> CheckUserExistsAsync(IUser user, CancellationToken cancellation);

        Task<int> UpdateUserPasswordAsync(IUser user, CancellationToken cancellation);

        Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation);

        Task<int> RemoveAreaAsync(IUser user, int areaId, CancellationToken cancellation);
        Task<string> GetSaltByEmailAddressAsync(string email, CancellationToken cancellation);
    }
}
