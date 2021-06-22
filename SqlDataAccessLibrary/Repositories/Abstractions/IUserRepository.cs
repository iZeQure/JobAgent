using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> AuthenticateUserLoginAsync(User user, CancellationToken cancellation);

        Task<User> GetUserByAccessTokenAsync(string accessToken, CancellationToken cancellation);

        Task<bool> CheckUserExistsAsync(User user, CancellationToken cancellation);

        Task<int> UpdateUserPasswordAsync(User user, CancellationToken cancellation);
    }
}
