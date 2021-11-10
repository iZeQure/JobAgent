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

        public async Task<bool> AuthenticateUserLoginAsync(string email, string password)
        {
            var authUser = new AuthUser
            {
                Email = email,
                Password = password
            };

            var isAuthenticated = await _userRepository.AuthenticateUserLoginAsync(authUser);

            return isAuthenticated;
        }

        public async Task<bool> CheckUserExistsAsync(string email)
        {
            return await _userRepository.CheckUserExistsAsync(email);
        }

        public async Task<IUser> CreateAsync(IUser entity)
        {
            var user = await _userRepository.CreateAsync(entity);

            if (user is not null)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                user.ConsultantAreas.Clear();
                user.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        public async Task<IUser> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user is not null)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                user.ConsultantAreas.Clear();
                user.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        public async Task<IUser> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is not null)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                user.ConsultantAreas.Clear();
                user.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        public async Task<string> GetSaltByEmailAddressAsync(string email)
        {
            return await _userRepository.GetSaltByEmailAsync(email);
        }

        public async Task<IAuthUser> GetUserByAccessTokenAsync(string accessToken)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);

            if (user is not null && user is AuthUser authUser)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(authUser);

                authUser.ConsultantAreas.Clear();
                authUser.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        public async Task<List<IUser>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users is not null)
            {
                foreach (var user in users)
                {
                    var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                    if (consultantAreas is not null)
                    {
                        user.ConsultantAreas.Clear();
                        user.ConsultantAreas.AddRange(consultantAreas);
                    }
                }
            }

            return users;
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
            var user = await _userRepository.UpdateAsync(entity);

            if (user is not null)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                user.ConsultantAreas.Clear();
                user.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        public async Task<bool> UpdateUserPasswordAsync(IAuthUser user)
        {
            return await _userRepository.UpdateUserPasswordAsync(user);
        }
    }
}
