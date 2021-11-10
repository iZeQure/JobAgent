using JobAgentClassLibrary.Common.Filters.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public interface IDynamicSearchFilterService
    {
        Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity);
        Task<List<IDynamicSearchFilter>> GetAllAsync();
        Task<IDynamicSearchFilter> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IDynamicSearchFilter entity);
        Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity);
    }
}
