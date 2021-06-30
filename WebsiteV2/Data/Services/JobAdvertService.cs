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
    public class JobAdvertService : BaseService<IJobAdvertRepository, JobAdvert>, IJobAdvertService
    {
        public JobAdvertService(IJobAdvertRepository repository) : base(repository)
        {
        }

        public async override Task<int> CreateAsync(JobAdvert createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public async override Task<int> DeleteAsync(JobAdvert deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public async override Task<IEnumerable<JobAdvert>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public async override Task<JobAdvert> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public async override Task<int> UpdateAsync(JobAdvert updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }
    }
}
