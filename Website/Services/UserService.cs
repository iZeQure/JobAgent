using System;
using System.Threading.Tasks;
using DataAccess;
using Pocos;
using JobAgent.Models;
using JobAgent.Services.Interfaces;
using JobAgent.Data.Providers;

namespace JobAgent.Services
{
    public class UserService : IUserService
    {
        private readonly DataAccessManager _dataAccessManager;
        private readonly SecurityProvider _securityProvider;

        public UserService()
        {
            _dataAccessManager = new();
            _securityProvider = new();
        }

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            return await _dataAccessManager.UserDataAccessManager().GetUserByAccessToken(accessToken);
        }

        public async Task<User> LoginAsync(User user)
        {
            // Check the user exists.
            if (await _dataAccessManager.UserDataAccessManager().CheckUserExists(user.Email))
            {
                // Get salt to hash password.
                user.Salt = await _dataAccessManager.UserDataAccessManager().GetUserSaltByEmail(user.Email);

                if (user.Salt != string.Empty)
                {
                    // Hash password with salt.
                    user.Password = _securityProvider.HashPassword(user.Password, user.Salt);

                    // Validate password with server.
                    if (await _dataAccessManager.UserDataAccessManager().ValidatePassword(user.Password))
                    {
                        var returnedUser = await _dataAccessManager.UserDataAccessManager().LogIn(user.Email, user.Password);
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
            var validation = await _dataAccessManager.UserDataAccessManager().CheckUserExists(email);

            if (validation)
            {
                return validation;
            }

            return validation;
        }

        public async Task<User> RegisterUserAsync(RegisterAccountModel registerAccountModel)
        {
            // Generate new salt.
            string salt = _securityProvider.GetNewSalt();

            // Hash user's password.
            string hashedSecret = _securityProvider.HashPassword(registerAccountModel.Password, salt);

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
                await _dataAccessManager.UserDataAccessManager().Create(user);
            }
            catch (Exception)
            {
                throw;
            }

            return user;
        }

        private string GenerateAccessToken(User user)
        {
            return _securityProvider.GenerateAccessToken(user);
        }
    }
}
