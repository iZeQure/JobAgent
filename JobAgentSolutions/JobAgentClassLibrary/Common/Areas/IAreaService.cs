using JobAgentClassLibrary.Common.Areas.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas
{
    public interface IAreaService
    {
        Task<IArea> CreateAsync(IArea entity);
        Task<List<IArea>> GetAllAsync();
        Task<IArea> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IArea entity);
        Task<IArea> UpdateAsync(IArea entity);
    }
}
