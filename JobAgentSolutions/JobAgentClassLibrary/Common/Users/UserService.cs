using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateUserLoginAsync(IAuthUser user)
        {
            return await _userRepository.AuthenticateUserLoginAsync(user);
        }

        public async Task<bool> CheckUserExistsAsync(IUser user)
        {
            return await _userRepository.CheckUserExistsAsync(user);
        }

        public async Task<IUser> CreateAsync(IUser entity)
        {
            return await _userRepository.CreateAsync(entity);
        }

        public async Task<IUser> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IUser> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<string> GetSaltByEmailAddressAsync(string email)
        {
            return await _userRepository.GetSaltByEmailAsync(email);
        }

        public async Task<IUser> GetUserByAccessTokenAsync(string accessToken)
        {
            return await _userRepository.GetUserByAccessTokenAsync(accessToken);
        }

        public async Task<List<IUser>> GetUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<int> GrantAreaToUserAsync(IUser user, int areaId)
        {
            return await _userRepository.GrantAreaToUserAsync(user, areaId);
        }

        public async Task<bool> RemoveAsync(IUser entity)
        {
            return await _userRepository.RemoveAsync(entity);
        }

        public async Task<int> RevokeAreaFromUserAsync(IUser user, int areaId)
        {
            return await _userRepository.RevokeAreaFromUserAsync(user, areaId);
        }

        public async Task<IUser> UpdateAsync(IUser entity)
        {
            return await _userRepository.UpdateAsync(entity);
        }

        public async Task<int> UpdateUserPasswordAsync(IAuthUser user)
        {
            return await _userRepository.UpdateUserPasswordAsync(user);
        }
    }
}
