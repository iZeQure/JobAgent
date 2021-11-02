using JobAgentClassLibrary.Common.Categories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories
{
    public interface ICategoryService
    {
        Task<List<ICategory>> GetMenuAsync();
        Task<ICategory> CreateAsync(ICategory entity);
        Task<List<ICategory>> GetCategoriesAsync();
        Task<ICategory> GetCategoryByIdAsync(int id);
        Task<bool> RemoveAsync(ICategory entity);
        Task<ICategory> UpdateAsync(ICategory entity);
        Task<ISpecialization> CreateAsync(ISpecialization entity);
        Task<List<ISpecialization>> GetSpecializationsAsync();
        Task<ISpecialization> GetSpecializationByIdAsync(int id);
        Task<bool> RemoveAsync(ISpecialization entity);
        Task<ISpecialization> UpdateAsync(ISpecialization entity);
    }
}
