using JobAgentClassLibrary.Common.Categories.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<ICategory> CreateAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICategory>> GetAllAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICategory> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICategory> UpdateAsync(ICategory entity)
        {
            throw new NotImplementedException();
        }
    }
}
