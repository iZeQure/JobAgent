using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Repositories;
using System;
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

        public async Task<IUser> GrantAreaToUserAsync(IUser user, int areaId)
        {
            try
            {
                var isGranted = await _userRepository.GrantUserConsultantAreaAsync(user, areaId);

                if (isGranted)
                {
                    var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                    if (consultantAreas is not null)
                    {
                        user.ConsultantAreas.Clear();
                        user.ConsultantAreas.AddRange(consultantAreas);

                        return user;
                    }

                    throw new Exception("Consultant area was granted. But user was not updated correctly.");
                }

                throw new Exception("Error occurred. Consultant area was NOT granted.");
            }
            catch (Exception)
            {
                // Any unhandled exceptions.
                throw;
            }
        }

        public async Task<bool> RemoveAsync(IUser entity)
        {
            return await _userRepository.RemoveAsync(entity);
        }

        public async Task<IUser> RevokeAreaFromUserAsync(IUser user, int areaId)
        {
            try
            {
                var isRevoked = await _userRepository.RevokeUserConsultantAreaAsync(user, areaId);

                if (isRevoked)
                {
                    var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                    if (consultantAreas is not null)
                    {
                        user.ConsultantAreas.Clear();
                        user.ConsultantAreas.AddRange(consultantAreas);

                        return user;
                    }

                    throw new Exception("Consultant area was revoked. But user was not updated correctly.");
                }

                throw new Exception("Error occurred. Consultant area was NOT revoked.");
            }
            catch (Exception)
            {
                // Any unhandled exceptions.
                throw;
            }
        }

        public async Task<IUser> UpdateAsync(IUser entity)
        {
            return await _userRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateUserPasswordAsync(IAuthUser user)
        {
            return await _userRepository.UpdateUserPasswordAsync(user);
        }
    }
}
