using JobAgentClassLibrary.Common.Users.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users
{
    public interface IUserService : IAuthUserService
    {
        Task<IUser> CreateAsync(IUser entity);

        Task<List<IUser>> GetUsersAsync();

        Task<IUser> GetUserByIdAsync(int id);

        Task<bool> RemoveAsync(IUser entity);

        Task<IUser> UpdateAsync(IUser entity);

        Task<IUser> GetByEmailAsync(string email);

        Task<bool> CheckUserExistsAsync(IUser user);

        Task<int> GrantAreaToUserAsync(IUser user, int areaId);

        Task<int> RevokeAreaFromUserAsync(IUser user, int areaId);
    }
}
