using JobAgent.Data.Objects;
using JobAgent.Data.Interfaces;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using System;
using System.Threading.Tasks;
using JobAgent.Models;

namespace JobAgent.Services
{
    public class UserService : IUserService
    {
        private IUserRepository UserRepository { get; }
        private SecurityService SecurityService { get; }

        public UserService()
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
                        returnedUser.AccessToken = returnedUser.AccessToken;

                        // Return authenticated user.
                        return await Task.FromResult(returnedUser);
                    }
                }
            }

            // Set authentication to false.
            user.IsAuthenticatedByServer = false;

            return await Task.FromResult(user);
        }

        public async Task<bool> CheckUserExistsAsync(string email)
        {
            if (UserRepository.CheckUserExists(email))
            {
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<User> RegisterUserAsync(RegisterAccountModel registerAccountModel)
        {
            // Generate new salt.
            string salt = (await SecurityService.GetNewSaltAsync());

            // Hash user's password.
            string hashedSecret = (await SecurityService.HashPasswordAsync(registerAccountModel.Password, salt));

            var user = new User()
            {
                Email = registerAccountModel.Email,
                FirstName = registerAccountModel.FirstName,
                LastName = registerAccountModel.LastName,
                ConsultantArea = new ConsultantArea() { Id = registerAccountModel.ConsultantAreaId },
                Location = new Location() { Id = registerAccountModel.LocationId },
                Salt = salt,
                Password = hashedSecret
            };

            string jwtToken = (await GenerateAccessTokenAsync(user));

            user.AccessToken = jwtToken;

            try
            {
                UserRepository.Create(user);
            }
            catch (Exception)
            {
                return await Task.FromResult(new User());
            }

            return await Task.FromResult(user);
        }

        private async Task<string> GenerateAccessTokenAsync(User user)
        {
            return await SecurityService.GenerateAccessTokenAsync(user);
        }
    }
}
