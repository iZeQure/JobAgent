using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts.Repositories
{
    public interface IJobAdvertRepository : IRepository<IJobAdvert, int>
    {
        Task<int> GetJobAdvertCountByCategoryId(int id);
        Task<int> GetJobAdvertCountBySpecializationId(int id);
        Task<int> GetJobAdvertCountByUncategorized();
    }
}
