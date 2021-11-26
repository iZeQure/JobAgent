using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Repositories;
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

        public UserService(IUserRepository userRepository, IAuthenticationAccess authAccess)
        {
            _userRepository = userRepository;
            _authAccess = authAccess;
        }

        /// <summary>
        /// Authenticates a user on login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Authuser object containing the users data</returns>
        public async Task<IAuthUser> AuthenticateUserLoginAsync(string email, string password)
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
                    throw new ArgumentException("Coudln't authenticate user, error while generating token.", nameof(email));
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

                    //auth.AccessToken = authUser.AccessToken;

                    return authenticatedUser;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if a user exists in the system
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserExistsAsync(string email)
        {
            return await _userRepository.CheckUserExistsAsync(email);
        }

        /// <summary>
        /// Creates a new User in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
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

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the salt of a User based off of a give email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GetSaltByEmailAddressAsync(string email)
        {
            return await _userRepository.GetSaltByEmailAsync(email);
        }

        /// <summary>
        /// Returns a specific User from the database
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<IAuthUser> GetUserByAccessTokenAsync(string accessToken)
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

        /// <summary>
        /// Returns a list of all Users in the database
        /// </summary>
        /// <returns></returns>
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
            catch (Exception)
            {
                // Any unhandled exceptions.
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
            return await _userRepository.RemoveAsync(entity);
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
            catch (Exception)
            {
                // Any unhandled exceptions.
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
            var user = await _userRepository.UpdateAsync(entity);

            if (user is not null)
            {
                var consultantAreas = await _userRepository.GetUserConsultantAreasAsync(user);

                user.ConsultantAreas.Clear();
                user.ConsultantAreas.AddRange(consultantAreas);
            }

            return user;
        }

        /// <summary>
        /// Updates a Users password in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUserPasswordAsync(IAuthUser user)
        {
            return await _userRepository.UpdateUserPasswordAsync(user);
        }

        /// <summary>
        /// Checks if a users accesstoken is valid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUserAccessTokenAsync(string accessToken)
        {
            var tokenIsValid = await _userRepository.ValidateUserAccessTokenAsync(accessToken);

            return tokenIsValid;
        }

        /// <summary>
        /// Returns a list of all a Users ConsultantAreas
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<IArea>> GetUserConsultantAreasAsync(IUser user)
        {
            return await _userRepository.GetUserConsultantAreasAsync(user);
        }
    }
}
