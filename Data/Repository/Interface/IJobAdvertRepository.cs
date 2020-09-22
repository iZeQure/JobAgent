using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Models;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository.Interface
{
    interface IJobAdvertRepository : IRepository<JobAdvert>
    {
        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsForAdmins();

        public Task<JobAdvert> GetJobAdvertDetailsForAdminsById(int id);

        public Task<int> GetCountOfJobAdvertsByCategoryId(int id);

        public Task<int> GetCountOfJobAdvertsBySpecializationId(int id);

        public Task<int> GetCountOfJobAdvertsUncategorized();
    }
}
