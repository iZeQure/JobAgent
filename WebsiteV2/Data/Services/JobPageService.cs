using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class JobPageService : BaseService<IJobPageRepository, JobPage>, IJobPageService
    {
        public JobPageService(IJobPageRepository jobPageRepository) : base(jobPageRepository)
        {

        }

        public async override Task<int> CreateAsync(JobPage createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public async override Task<int> DeleteAsync(JobPage deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public async override Task<IEnumerable<JobPage>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public async override Task<JobPage> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public async override Task<int> UpdateAsync(JobPage updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }
    }
}
