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
            IRepository<JobAdvertCategory> repository = new JobAdvertCategoryRepository();

            return Task.FromResult(repository.GetAll().ToList());
        }

        public Task<List<JobAdvert>> GetJobAdvertsAsync(string categoryName)
        {
            List<JobAdvert> temp = new List<JobAdvert>();

            temp = (from j in temp
                   where j.JobAdvertCategoryId.Name == categoryName
                   select j).ToList();

            return Task.FromResult(temp);
        }
    }
}
