using JobAgentClassLibrary.Common.Categories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories
{
    public interface ICategoryService
    {
        Task<ICategory> CreateAsync(ICategory entity);
        Task<List<ICategory>> GetAllAsync();
        Task<ICategory> GetByIdAsync(int id);
        Task<bool> RemoveAsync(ICategory entity);
        Task<ICategory> UpdateAsync(ICategory entity);
    }
}
