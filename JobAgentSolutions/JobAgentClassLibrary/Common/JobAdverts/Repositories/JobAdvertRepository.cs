using JobAgentClassLibrary.Common.JobAdverts.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts.Repositories
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        public Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<IJobAdvert>> GetAllAsync(IJobAdvert entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<JobAdvert>> GetAllUncategorized()
        {
            throw new NotImplementedException();
        }

        public Task<IJobAdvert> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetJobAdvertCountByUncategorized()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IJobAdvert entity)
        {
            throw new NotImplementedException();
        }

        public Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            throw new NotImplementedException();
        }
    }
}
