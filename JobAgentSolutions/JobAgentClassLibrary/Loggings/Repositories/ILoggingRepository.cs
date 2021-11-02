using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Loggings.Entities;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public interface ILoggingRepository : IRepository<ILog, int>
    {

    }
}
