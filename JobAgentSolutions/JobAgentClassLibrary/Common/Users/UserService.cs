using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Security.interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationAccess _authAccess;
        private readonly ILogService _logService;

        public UserService(IUserRepository userRepository, IAuthenticationAccess authAccess, ILogService logService)
        {
            _userRepository = userRepository;
            _authAccess = authAccess;
            _logService = logService;
        }

        /// <summary>
        /// Authenticates a user on login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Authuser object containing the users data</returns>
        public async Task<IAuthUser> AuthenticateUserLoginAsync(string email, string password)
        {
            try
            {
                var authUser = new AuthUser
                {
                    Email = email,
                    Password = password
                };

                var isAuthenticated = await _userRepository.AuthenticateUserLoginAsync(authUser);

                if (isAuthenticated)
                {
                    authUser.AccessToken = await _authAccess.GenerateAccessTokenAsync(authUser);
                    var tokenUpdated = await _userRepository.UpdateUserAccessTokenAsync(authUser);

                    if (!tokenUpdated)
                    {
                        throw new ArgumentException("Couldn't authenticate user, error while generating token.", nameof(email));
                    }

                    var returnedUser = await _userRepository.GetByEmailAsync(email);

                    if (returnedUser is not null)
                    {
                        var areas = await _userRepository.GetUserConsultantAreasAsync(returnedUser);

                        var authenticatedUser = new AuthUser
                        {
                            Id = returnedUser.Id,
                            RoleId = returnedUser.RoleId,
                            LocationId = returnedUser.LocationId,
                            FirstName = returnedUser.FirstName,
                            LastName = returnedUser.LastName,
                            Email = returnedUser.Email,
                            ConsultantAreas = areas,
                            AccessToken = authUser.AccessToken
                        };

                        return authenticatedUser;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to authenticate user", nameof(AuthenticateUserLoginAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Checks if a user exists in the system
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserExistsAsync(string email)
        {
            try
            {
                return await _userRepository.CheckUserExistsAsync(email);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Creates a new User in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IUser> CreateAsync(IUser entity)
        {
            try
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create user", nameof(CreateAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<IUser> GetByEmailAsync(string email)
        {
            try
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get user by email", nameof(GetByEmailAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IUser> GetUserByIdAsync(int id)
        {
            try
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get user by id", nameof(GetUserByIdAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns the salt of a User based off of a give email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GetSaltByEmailAddressAsync(string email)
        {
            try
            {
                return await _userRepository.GetSaltByEmailAsync(email);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get salt", nameof(GetSaltByEmailAddressAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<IAuthUser> GetUserByAccessTokenAsync(string accessToken)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);

                if (user is not null && user is AuthUser authUser)
                {
                    var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(authUser);

                    authUser.ConsultantAreas = new();
                    authUser.ConsultantAreas.AddRange(consultantAreas);
                }

                return user;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get user by accesstoken", nameof(GetUserByAccessTokenAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all Users in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IUser>> GetUsersAsync()
        {
            try
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get users", nameof(GetUsersAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Grants a User a ConsultantArea
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to grant user area", nameof(GrantAreaToUserAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// removes a User from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IUser entity)
        {
            try
            {
                return await _userRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove user", nameof(RemoveAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a ConsultantArea from the User
        /// </summary>
        /// <param name="user"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to revoke area from user", nameof(RevokeAreaFromUserAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a User in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IUser> UpdateAsync(IUser entity)
        {
            try
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
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to Update user", nameof(UpdateAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a Users password in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPasswordAsync(IAuthUser user)
        {
            try
            {
                return await _userRepository.UpdateUserPasswordAsync(user);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update user password", nameof(UpdateUserPasswordAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Checks if a users accesstoken is valid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUserAccessTokenAsync(string accessToken)
        {
            try
            {
                var tokenIsValid = await _userRepository.ValidateUserAccessTokenAsync(accessToken);

                return tokenIsValid;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to validate user token", nameof(ValidateUserAccessTokenAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all a Users ConsultantAreas
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<IArea>> GetUserConsultantAreasAsync(IUser user)
        {
            try
            {
                return await _userRepository.GetUserConsultantAreasAsync(user);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get user ares", nameof(GetUserConsultantAreasAsync), nameof(UserService), LogType.SERVICE);
                throw;
            }
        }
    }
}
