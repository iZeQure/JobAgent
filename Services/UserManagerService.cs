using JobAgent.Data;
using JobAgent.Data.Interfaces;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class UserManagerService : IUserService
    {
        private IUserRepository UserRepository { get; }
        private SecurityService SecurityService { get; }

        public UserManagerService()
        {
            UserRepository = new UserRepository();
            SecurityService = new SecurityService();
        }

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            return await Task.FromResult(UserRepository.GetUserByAccessToken(accessToken));
        }

        public async Task<User> LoginAsync(User user)
        {
            // Check the user exists.
            if (UserRepository.CheckUserExists(user.Email))
            {
                // Get salt to hash password.
                user.Salt = UserRepository.GetUserSaltByEmail(user.Email);

                if (user.Salt != string.Empty)
                {
                    // Hash password with salt.
                    user.Password = await SecurityService.HashPasswordAsync(user.Password, user.Salt);

                    // Validate password with server.
                    if (UserRepository.ValidatePassword(user.Password))
                    {                        
                        var returnedUser = UserRepository.LogIn(user.Email, user.Password);
                        returnedUser.IsAuthenticatedByServer = true;
                        returnedUser.AccessToken = returnedUser.Email;

                        // Return authenticated user.
                        return await Task.FromResult(returnedUser);
                    }
                }
            }

            // Set authentication to false.
            user.IsAuthenticatedByServer = false;

            return await Task.FromResult(user);
        }

        public Task<User> RegisterUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> RereshTokenAsync(RefreshRequest refreshRequest)
        {
            throw new NotImplementedException();
        }
    }
}
