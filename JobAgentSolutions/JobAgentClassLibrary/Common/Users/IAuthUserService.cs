using JobAgentClassLibrary.Common.Users.Entities;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users
{
    public interface IAuthUserService
    {
        Task<bool> AuthenticateUserLoginAsync(IAuthUser user);

        Task<int> UpdateUserPasswordAsync(IAuthUser user);

        Task<IUser> GetUserByAccessTokenAsync(string accessToken);

        Task<string> GetSaltByEmailAddressAsync(string email);
    }
}
