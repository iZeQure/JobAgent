using JobAgentClassLibrary.Common.Filters.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public interface IStaticSearchFilterService
    {
        Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity);
        Task<List<IStaticSearchFilter>> GetAllAsync();
        Task<IStaticSearchFilter> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IStaticSearchFilter entity);
        Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity);
    }
}
