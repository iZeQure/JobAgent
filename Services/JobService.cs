using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;

namespace JobAgent.Services
{
    public class JobService
    {
        private IRepository<Category> CategoryRepository { get; } = new CategoryRepository();
        private IRepository<JobAdvert> AdvertRepository { get; } = new JobAdvertRepository();

        private List<JobAdvert> JobAdverts { get; set; } = new List<JobAdvert>();

        public Task<List<Category>> GetJobMenuAsync()
        {
            var jobMenu = ((ICategoryRepository)CategoryRepository).GetAllCategoriesWithSpecializations();

            return Task.FromResult(jobMenu);
        }

        public async Task<List<JobAdvert>> GetJobVacanciesAsync(int id, string name)
        {
            JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            var sortJobs = from job in JobAdverts
                           where job.JobAdvertCategoryId.Id == id
                           orderby job.JobRegisteredDate ascending
                           select job;

            return await Task.FromResult(sortJobs.ToList());
        }

        public async Task<List<JobAdvert>> GetJobSpecialVacanciesAsync(int id, string name)
        {
            JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            var sortJobs = from job in JobAdverts
                           where job.JobAdvertCategorySpecializationId.Id == id
                           orderby job.JobRegisteredDate ascending
                           select job;

            return await Task.FromResult(sortJobs.ToList());
        }
    }
}
