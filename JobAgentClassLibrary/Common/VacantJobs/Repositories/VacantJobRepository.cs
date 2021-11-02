using JobAgentClassLibrary.Common.VacantJobs.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs.Repositories
{
    public class VacantJobRepository : IVacantJobRepository
    {
        public Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IVacantJob>> GetAllAsync(IVacantJob entity)
        {
            throw new NotImplementedException();
        }

        public Task<IVacantJob> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IVacantJob entity)
        {
            throw new NotImplementedException();
        }

        public Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            throw new NotImplementedException();
        }
    }
}
