using JobAgentClassLibrary.Common.Filters.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public interface IFilterTypeService
    {
        Task<IFilterType> CreateAsync(IFilterType entity);
        Task<List<IFilterType>> GetAllAsync();
        Task<IFilterType> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IFilterType entity);
        Task<IFilterType> UpdateAsync(IFilterType entity);
    }
}
