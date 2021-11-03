using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Common.Users.Entities;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public interface IUserRepository : IRepository<IUser, int>
    {
        Task<bool> AuthenticateUserLoginAsync(IAuthUser user);

        Task<IUser> GetUserByAccessTokenAsync(string accessToken);

        Task<IUser> GetByEmailAsync(string email);

        Task<bool> CheckUserExistsAsync(IUser user);

        Task<int> UpdateUserPasswordAsync(IAuthUser user);

        Task<int> GrantAreaToUserAsync(IUser user, int areaId);

        Task<int> RevokeAreaFromUserAsync(IUser user, int areaId);

        Task<string> GetSaltByEmailAddressAsync(string email);
    }
}
