using JobAgentClassLibrary.Loggings.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings
{
    public interface ILogService
    {
        Task<ILog> CreateAsync(ILog entity);
        Task<List<ILog>> GetAllDbLogsAsync();
        Task<List<ILog>> GetAllCrawlerLogsAsync();
        Task<ILog> GetByIdAsync(int id);
        Task<bool> RemoveAsync(ILog entity);
        Task<ILog> UpdateAsync(ILog entity);
    }
}
