using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;

namespace JobAgent.Services
{
    public class JobService
    {
        private IRepository<JobAdvertCategory> CategoryRepository { get; } = new JobAdvertCategoryRepository();
        private IRepository<JobAdvert> AdvertRepository { get; } = new JobAdvertRepository();

        private List<JobAdvert> JobAdverts { get; set; } = new List<JobAdvert>();

        public Task<List<JobAdvertCategory>> GetJobMenuAsync()
        {
            var jobMenu = ((IJobAdvertCategoryRepository)CategoryRepository).GetAllJobAdvertCategoriesWithSpecializations();

            return Task.FromResult(jobMenu);
        }

        public async Task<List<JobAdvert>> GetJobVacanciesAsync(int id, string name)
        {
            JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            var sortJobs = from job in JobAdverts
                           where job.JobAdvertCategoryId.Id == id
                           select job;

            return await Task.FromResult(sortJobs.ToList());
        }

        public async Task<List<JobAdvert>> GetJobSpecialVacanciesAsync(int id, string name)
        {
            JobAdverts = await Task.FromResult(AdvertRepository.GetAll().ToList());

            var sortJobs = from job in JobAdverts
                           where job.JobAdvertCategorySpecializationId.Id == id
                           select job;

            return await Task.FromResult(sortJobs.ToList());
        }
    }
}
