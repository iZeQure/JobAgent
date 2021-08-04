using BlazorServerWebsite.Data.FormModels;
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

        public async Task<JobAdvertPaginationModel> FilteredJobAdvertPagination(CancellationToken cancellation, int sortByCategoryId, int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdverts = (await Repository.GetAllAsync(cancellation)).Where(c => c.Category.Id == sortByCategoryId).OrderBy(x => x.Id),
                CurrentPage = page,
                JobAdvertsPerPage = 25
            };

            return paginationModel;
        }

        public async override Task<IEnumerable<JobAdvert>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public async override Task<JobAdvert> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public async Task<int> GetJobAdvertCountByCategoryId(int id, CancellationToken cancellation)
        {
            return await Repository.GetJobAdvertCountByCategoryId(id, cancellation);
        }

        public async Task<int> GetJobAdvertCountBySpecializationId(int id, CancellationToken cancellation)
        {
            return await Repository.GetJobAdvertCountBySpecializationId(id, cancellation);
        }

        public async Task<int> GetJobAdvertCountByUncategorized(CancellationToken cancellation)
        {
            return await Repository.GetJobAdvertCountByUncategorized(cancellation);
        }

        public async Task<JobAdvertPaginationModel> JobAdvertPagination(CancellationToken cancellation, int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = 25,
                JobAdverts = (await Repository.GetAllAsync(cancellation)).OrderBy(x => x.RegistrationDateTime),
                CurrentPage = page
            };

            return paginationModel;
        }

        public async Task<JobAdvertPaginationModel> JobAdvertPagination(CancellationToken cancellation, int resultsPerPage, int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = resultsPerPage,
                JobAdverts = (await Repository.GetAllAsync(cancellation)).OrderBy(x => x.RegistrationDateTime),
                CurrentPage = page
            };

            return paginationModel;
        }

        public async override Task<int> UpdateAsync(JobAdvert updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }
    }
}
