using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Common.Users.Entities;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public interface IUserRepository : IRepository<IUser, int>
    {
        Task<bool> AuthenticateUserLoginAsync(IUser user);

        Task<IUser> GetUserByAccessTokenAsync(string accessToken);

        Task<IUser> GetByEmailAsync(string email);

        Task<bool> CheckUserExistsAsync(IUser user);

        Task<int> UpdateUserPasswordAsync(IUser user);

        Task<int> GrantUserAreaAsync(IUser user, int areaId);

        Task<int> RemoveAreaAsync(IUser user, int areaId);

        Task<string> GetSaltByEmailAddressAsync(string email);
    }
}
