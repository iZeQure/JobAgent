using DataAccess.Repositories.Base;
using Pocos;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByAccessToken(string accessToken);
        Task<User> LogIn(string email, string password);
        Task<bool> CheckUserExists(string email);
        Task<bool> ValidatePassword(string password);
        Task<string> GetUserSaltByEmail(string email);

        /// <summary>
        /// Update users password.
        /// </summary>
        /// <param name="authorization">Used to identify user with new password.</param>
        void UpdateUserPassword(User authorization);
    }
}
