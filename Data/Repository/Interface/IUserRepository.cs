﻿using JobAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string email);
        User GetUserByAccessToken(string accessToken);
        User LogIn(string email, string password);
        bool CheckUserExists(string email);
        bool ValidatePassword(string password);
        string GetUserSaltByEmail(string email);
        RefreshTokenModel GenerateRefreshToken();
        string GenerateAccessToken(int id);
    }
}
