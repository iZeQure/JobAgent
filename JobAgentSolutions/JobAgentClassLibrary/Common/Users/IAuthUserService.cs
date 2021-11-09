using JobAgentClassLibrary.Common.Users.Entities;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users
{
    public interface IAuthUserService
    {
        Task<bool> AuthenticateUserLoginAsync(IAuthUser user);

        Task<bool> UpdateUserPasswordAsync(IAuthUser user);

        Task<IAuthUser> GetUserByAccessTokenAsync(string accessToken);

        Task<string> GetSaltByEmailAddressAsync(string email);
    }
}
