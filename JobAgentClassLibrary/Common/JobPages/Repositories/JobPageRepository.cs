using JobAgentClassLibrary.Common.JobPages.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages.Repositories
{
    public class JobPageRepository : IJobPageRepository
    {
        public Task<IJobPage> CreateAsync(IJobPage entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IJobPage>> GetAllAsync(IJobPage entity)
        {
            throw new NotImplementedException();
        }

        public Task<IJobPage> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IJobPage entity)
        {
            throw new NotImplementedException();
        }

        public Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            throw new NotImplementedException();
        }
    }
}
