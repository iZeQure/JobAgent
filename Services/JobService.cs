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
        public Task<List<JobAdvertCategory>> GetJobMenuAsync()
        {
            IJobAdvertCategoryRepository repository = new JobAdvertCategoryRepository();

            var jobMenu = repository.GetAllJobAdvertCategoriesWithSpecializations();

            return Task.FromResult(jobMenu);
        }

        public Task<List<JobAdvert>> GetJobAdvertsAsync(int jobAdvertId)
        {
            IRepository<JobAdvert> repository = new JobAdvertRepository();

            List<JobAdvert> temp = repository.GetAll().ToList();

            temp = (from j in temp
                   where j.Id == jobAdvertId
                   select j).ToList();

            return Task.FromResult(temp);
        }
    }
}
