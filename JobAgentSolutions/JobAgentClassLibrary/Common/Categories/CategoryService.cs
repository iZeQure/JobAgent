using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISpecializationRepository _specializationRepository;

        public CategoryService(ICategoryRepository categoryRepository, ISpecializationRepository specializationRepository)
        {
            _categoryRepository = categoryRepository;
            _specializationRepository = specializationRepository;
        }

        public async Task<ICategory> CreateAsync(ICategory entity)
        {
            return await _categoryRepository.CreateAsync(entity);
        }

        public async Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            return await _specializationRepository.CreateAsync(entity);
        }

        public async Task<List<ICategory>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<ICategory> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task<List<ICategory>> GetMenuAsync()
        {
            var categoriesTask = _categoryRepository.GetAllAsync();
            var specializationsTask = _specializationRepository.GetAllAsync();

            try
            {
                await Task.WhenAll(categoriesTask, specializationsTask);
            }
            catch (Exception)
            {
                throw;
            }

            var categories = categoriesTask.Result;
            var specializations = specializationsTask.Result;

            foreach (var c in categories)
            {
                if (c.Id is 0) continue;

                var matches = specializations.Where(s => s.CategoryId == c.Id);

                if (!matches.Any()) continue;

                c.AddRange(matches);
            }

            return categories;
        }

        public async Task<ISpecialization> GetSpecializationByIdAsync(int id)
        {
            return await _specializationRepository.GetByIdAsync(id);
        }

        public async Task<List<ISpecialization>> GetSpecializationsAsync()
        {
            return await _specializationRepository.GetAllAsync();
        }

        public async Task<bool> RemoveAsync(ICategory entity)
        {
            return await _categoryRepository.RemoveAsync(entity);
        }

        public async Task<bool> RemoveAsync(ISpecialization entity)
        {
            return await _specializationRepository.RemoveAsync(entity);
        }

        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            return await _categoryRepository.UpdateAsync(entity);
        }

        public async Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            return await _specializationRepository.UpdateAsync(entity);
        }
    }
}
