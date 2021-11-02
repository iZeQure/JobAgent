using JobAgentClassLibrary.Common.Roles.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Roles.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public Task<IRole> CreateAsync(IRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IRole>> GetAllAsync(IRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IRole entity)
        {
            throw new NotImplementedException();
        }

        public Task<IRole> UpdateAsync(IRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
