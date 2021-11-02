using JobAgentClassLibrary.Common.Users.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> AuthenticateUserLoginAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckUserExistsAsync(IUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> CreateAsync(IUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IUser>> GetAllAsync(IUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSaltByEmailAddressAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> GetUserByAccessTokenAsync(string accessToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GrantUserAreaAsync(IUser user, int areaId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAreaAsync(IUser user, int areaId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> UpdateAsync(IUser entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateUserPasswordAsync(IUser user)
        {
            throw new NotImplementedException();
        }
    }
}
