using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.VacantJobs.Repositories
{
    public interface IVacantJobRepository : IRepository<IVacantJob, int>
    {
    }
}
