using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Models;
using Pocos;

namespace JobAgent.Data.Interfaces
{
    public interface IUserService
    {
        public Task<User> LoginAsync(User user);
        public Task<User> RegisterUserAsync(RegisterAccountModel registerAccountModel);
        public Task<User> GetUserByAccessToken(string accessToken);
    }
}
