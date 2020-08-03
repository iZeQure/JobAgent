using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface IUserRepository : IRepository<User>
    {
        bool LogIn(string email, string password);
        bool CheckUserExists(string email);
        bool ValidatePassword(string password);
        string GetUserSaltByEmail(string email);
    }
}
