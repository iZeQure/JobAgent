using JobAgentClassLibrary.Common.Filters.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public class DynamicSearchFilterRepository : IDynamicSearchFilterRepository
    {
        public Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IDynamicSearchFilter>> GetAllAsync(IDynamicSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IDynamicSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            throw new NotImplementedException();
        }
    }
}
