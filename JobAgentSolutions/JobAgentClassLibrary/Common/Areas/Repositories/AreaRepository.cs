using JobAgentClassLibrary.Common.Areas.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        public Task<IArea> CreateAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IArea>> GetAllAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<IArea> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IArea entity)
        {
            throw new NotImplementedException();
        }

        public Task<IArea> UpdateAsync(IArea entity)
        {
            throw new NotImplementedException();
        }
    }
}
