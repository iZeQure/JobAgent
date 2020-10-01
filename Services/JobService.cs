using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using JobAgent.Models;

namespace JobAgent.Services
{
    public class JobService
    {
        private protected IRepository<Category> CategoryRepository { get; } = new CategoryRepository();
        private protected IRepository<JobAdvert> AdvertRepository { get; } = new JobAdvertRepository();

        public Task<JobAdvertPaginationModel> JobAdvertPagination(int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = 25,
                JobAdverts = AdvertRepository.GetAll().OrderBy(x => x.JobRegisteredDate),
                CurrentPage = page
            };

            return Task.FromResult(paginationModel);
        }

        public Task<JobAdvertPaginationModel> JobAdvertPagination(int perPage, int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = perPage,
                JobAdverts = AdvertRepository.GetAll().OrderBy(x => x.JobRegisteredDate),
                CurrentPage = page
            };

            return Task.FromResult(paginationModel);
        }

        public Task<JobAdvertPaginationModel> FilterJobAdvertPagination(int sortedByCategoryId, int pageNumber = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdverts = AdvertRepository.GetAll().Where(c => c.Category.Id == sortedByCategoryId).OrderBy(x => x.Id),
                CurrentPage = pageNumber,
                JobAdvertsPerPage = 25
            };

            return Task.FromResult(paginationModel);
        }

        public Task<JobAdvert> GetJobVacancyById(int advertId)
        {
            return Task.FromResult(AdvertRepository.GetById(advertId));
        }

        public Task<List<Category>> GetJobMenuAsync()
        {
            return ((ICategoryRepository)CategoryRepository).GetAllCategoriesWithSpecializations();
        }

        public async Task<List<JobAdvert>> GetUncategorizedJobVacancies()
        {
            var list = await Task.FromResult(AdvertRepository.GetAll());

            var uncatedgorizedJobs = from job in list
                                     where job.Category.Id == 0
                                     orderby job.JobVacancyRegisteredAgo ascending
                                     select job;

            return await Task.FromResult(result: uncatedgorizedJobs as List<JobAdvert>);
        }

        public async Task<IEnumerable<JobAdvert>> GetJobVacanciesAsync(int id)
        {
            return await ((IJobAdvertRepository)AdvertRepository).GetAllJobAdvertsSortedByCategoryId(id);

            //JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            //var sortJobs = from job in JobAdverts
            //               where job.Category.Id == id
            //               orderby job.JobRegisteredDate ascending
            //               select job;

            //return await Task.FromResult(result: sortJobs.ToList());
        }

        public async Task<IEnumerable<JobAdvert>> GetJobSpecialVacanciesAsync(int id)
        {
            return await ((IJobAdvertRepository)AdvertRepository).GetAllJobAdvertsSortedBySpecializationId(id);

            //JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            //var sortJobs = from job in JobAdverts
            //               where job.Specialization.Id == id
            //               orderby job.JobRegisteredDate ascending
            //               select job;

            //return await Task.FromResult(result: sortJobs.ToList());
        }
    }
}
