using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.JobAdverts.Repositories
{
    public interface IJobAdvertRepository : IRepository<IJobAdvert, int>
    {
    }
}
