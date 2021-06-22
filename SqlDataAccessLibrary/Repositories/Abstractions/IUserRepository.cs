using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IUser> AuthenticateUserLoginAsync(User user);

        Task<User> GetUserByAccessTokenAsync(string accessToken);

        Task<bool> CheckUserExistsAsync(User user);

        Task<int> UpdateUserPasswordAsync(User user);
    }
}
