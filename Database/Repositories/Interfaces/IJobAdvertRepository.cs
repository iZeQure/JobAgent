using DataAccess.Repositories.Base;
using Pocos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IJobAdvertRepository : IRepository<JobAdvert>
    {
        public Task<IEnumerable<JobAdvert>> GetAllUncategorized();

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedByCategoryId(int categoryId);

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedBySpecializationId(int specializationId);

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsForAdmins();

        public Task<JobAdvert> GetJobAdvertDetailsForAdminsById(int id);

        public Task<int> GetCountOfJobAdvertsByCategoryId(int id);

        public Task<int> GetCountOfJobAdvertsBySpecializationId(int id);

        public Task<int> GetCountOfJobAdvertsUncategorized();
    }
}
