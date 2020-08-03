using JobAgent.Data;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class AuthenticationService
    {
        public async Task<UserManagerResponse> LogInUserAsync(LogInModel model)
        {
            IUserRepository userRepository = new UserRepository();
            UserManagerResponse response = new UserManagerResponse();

            // Check if the user exists.
            if (userRepository.CheckUserExists(model.Email))
            {
                // Get user salt.
                string s = userRepository.GetUserSaltByEmail(model.Email);

                // Check if empty.
                if (s != string.Empty)
                {
                    // Hash password for validation.
                    SecurityService securityService = new SecurityService();

                    string hashedPassword = await securityService.HashPasswordAsync(model.Password, s);

                    // Validate password.
                    if (userRepository.ValidatePassword(hashedPassword))
                    {
                        response.IsSuccess = true;
                        response.ExpireDate = DateTime.Now.AddDays(14);
                        response.Message = "Logged in";

                        return response;
                    }
                }

                response.IsSuccess = false;
                response.Message = "Email eller adgangskode er forkert.";

                return response;
            }

            response.IsSuccess = false;
            response.Message = "Email eller adgangskode er forkert.";

            return response;

            //var response = new UserManagerResponse()
            //{
            //    UserInfo = new User()
            //    {
            //        Id = 1,
            //        Email = model.Email,
            //        Password = model.Password
            //    },
            //    Message = "Success",
            //    IsSuccess = true,
            //    ExpireDate = DateTime.Now.AddDays(14)
            //};

            //return response;
        }
    }
}
