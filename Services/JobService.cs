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

        public Task<List<JobAdvert>> GetJobAdvertsAsync(int jobAdvertId, int jobAdvertSpecId, string jobAdvertName)
        {
            IRepository<JobAdvert> repository = new JobAdvertRepository();

            List<JobAdvert> temp = repository.GetAll().ToList();

            if (temp.Any(x => x.JobAdvertCategoryId.Id == jobAdvertId && x.JobAdvertCategorySpecializationId.Id == 0))
            {
                temp = (from j in temp
                        where j.JobAdvertCategoryId.Id == jobAdvertId
                        select j
                        ).ToList(); 
            }
            else
            {
                temp = (from j in temp
                        where j.JobAdvertCategorySpecializationId.Id == jobAdvertSpecId
                        select j
                        ).ToList();
            }

            //if (temp.Any(x => x.JobAdvertCategorySpecializationId.Id == 0))
            //{
            //    temp = (from j in temp
            //            where j.JobAdvertCategoryId.Id == jobAdvertId
            //            select j).ToList();
            //}
            //else if (temp.Any(x => x.JobAdvertCategorySpecializationId.Name == jobAdvertName))
            //{
            //    temp = (from j in temp
            //            where j.JobAdvertCategorySpecializationId.Name == jobAdvertName && j.JobAdvertCategoryId.Id == jobAdvertId
            //            select j).ToList();
            //}            

            return Task.FromResult(temp);
        }

        /*
         * 
         *  Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Email = reader.GetString(2),
            PhoneNumber = reader.GetString(3),
            JobDescription = reader.GetString(4),
            JobLocation = reader.GetString(5),
            JobRegisteredDate = reader.GetDateTime(6),
            DeadlineDate = reader.GetDateTime(7),
            CompanyCVR = new Company()
            {
                Id = reader.GetInt32(8)
            },
            JobAdvertCategoryId = new JobAdvertCategory()
            {
                Id = reader.GetInt32(9)
            },
            JobAdvertCategorySpecializationId = new JobAdvertCategorySpecialization()
            {
                Id = reader.GetInt32(10)
            }
         * 
         */
    }
}
