using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ICategory> CreateAsync(ICategory entity)
        {
            return await _categoryRepository.CreateAsync(entity);
        }

        public async Task<List<ICategory>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<ICategory> GetByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(ICategory entity)
        {
            return await _categoryRepository.RemoveAsync(entity);
        }

        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            return await _categoryRepository.UpdateAsync(entity);
        }
    }
}
