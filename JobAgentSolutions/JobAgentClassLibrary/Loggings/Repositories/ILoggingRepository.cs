using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Loggings.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public interface ILoggingRepository : IRepository<ILog, int>
    {
        Task<List<ILog>> GetAllCrawlerLogsAsync();
    }
}
