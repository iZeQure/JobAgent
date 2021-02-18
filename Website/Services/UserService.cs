using System;
using System.Threading.Tasks;
using DataAccess;
using Pocos;
using JobAgent.Models;
using JobAgent.Services.Interfaces;

namespace JobAgent.Services
{
    public class UserService : IUserService
    {
        private DataAccessManager DataAccessManager { get; }
        private SecurityService SecurityService { get; }

        public UserService()
        {
            DataAccessManager = new DataAccessManager();
            SecurityService = new SecurityService();
        }

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            return await DataAccessManager.UserDataAccessManager().GetUserByAccessToken(accessToken);
        }

        public async Task<User> LoginAsync(User user)
        {
            // Check the user exists.
            if (await DataAccessManager.UserDataAccessManager().CheckUserExists(user.Email))
            {
                // Get salt to hash password.
                user.Salt = await DataAccessManager.UserDataAccessManager().GetUserSaltByEmail(user.Email);

                if (user.Salt != string.Empty)
                {
                    // Hash password with salt.
                    user.Password = SecurityService.HashPasswordAsync(user.Password, user.Salt);

                    // Validate password with server.
                    if (await DataAccessManager.UserDataAccessManager().ValidatePassword(user.Password))
                    {
                        var returnedUser = await DataAccessManager.UserDataAccessManager().LogIn(user.Email, user.Password);
                        returnedUser.IsAuthenticatedByServer = true;
                        returnedUser.AccessToken = returnedUser.AccessToken;

                        // Return authenticated user.
                        return returnedUser;
                    }
                }
            }

            // Set authentication to false.
            user.IsAuthenticatedByServer = false;

            return user;
        }

        public async Task<bool> CheckUserExistsAsync(string email)
        {
            var validation = await DataAccessManager.UserDataAccessManager().CheckUserExists(email);

            if (validation)
            {
                return validation;
            }

            return validation;
        }

        public async Task<User> RegisterUserAsync(RegisterAccountModel registerAccountModel)
        {
            // Generate new salt.
            string salt = SecurityService.GetNewSaltAsync();

            // Hash user's password.
            string hashedSecret = SecurityService.HashPasswordAsync(registerAccountModel.Password, salt);

            var user = new User()
            {
                Email = registerAccountModel.Email,
                FirstName = registerAccountModel.FirstName,
                LastName = registerAccountModel.LastName,
                ConsultantArea = new ConsultantArea() { Identifier = registerAccountModel.ConsultantAreaId },
                Location = new Location() { Identifier = registerAccountModel.LocationId },
                Salt = salt,
                Password = hashedSecret
            };

            string accessToken = GenerateAccessToken(user);

            user.AccessToken = accessToken;

            try
            {
                await DataAccessManager.UserDataAccessManager().Create(user);
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        private string GenerateAccessToken(User user)
        {
            return SecurityService.GenerateAccessToken(user);
        }
    }
}
