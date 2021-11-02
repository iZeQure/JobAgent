using JobAgentClassLibrary.Common.Filters.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public class StaticSearchFilterRepository : IStaticSearchFilterRepository
    {
        public Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IStaticSearchFilter>> GetAllAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }
    }
}
