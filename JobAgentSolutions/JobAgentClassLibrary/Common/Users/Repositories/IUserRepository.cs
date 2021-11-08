using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public interface IUserRepository : IRepository<IUser, int>
    {
        Task<IUser> GetUserByAccessTokenAsync(string accessToken);

        Task<IUser> GetByEmailAsync(string email);

        Task<bool> AuthenticateUserLoginAsync(IAuthUser user);

        Task<bool> CheckUserExistsAsync(IUser user);

        Task<string> GetSaltByEmailAsync(string email);

        Task<bool> UpdateUserPasswordAsync(IAuthUser user);

        Task<bool> GrantUserConsultantAreaAsync(IUser user, int areaId);

        Task<bool> RevokeUserConsultantAreaAsync(IUser user, int areaId);

        Task<IUser> GetUserConsultantAreasAsync(IUser user);
    }
}
