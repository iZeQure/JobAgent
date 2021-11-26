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

        /// <summary>
        /// Creates a new Category in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved object</returns>
        public async Task<ICategory> CreateAsync(ICategory entity)
        {
            return await _categoryRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Creates a new Specialization in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>returns the saved object</returns>
        public async Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            return await _specializationRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all Categories in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ICategory>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }

        /// <summary>
        /// Returns a list of a specific Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICategory> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Returns a list of categories matched with specializations for the sidebarmenu
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns a specific Specialization from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ISpecialization> GetSpecializationByIdAsync(int id)
        {
            return await _specializationRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Returns a list of all Specializations from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ISpecialization>> GetSpecializationsAsync()
        {
            return await _specializationRepository.GetAllAsync();
        }

        /// <summary>
        /// Removes a Category from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ICategory entity)
        {
            return await _categoryRepository.RemoveAsync(entity);
        }

        /// <summary>
        /// Removes a Specialization from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ISpecialization entity)
        {
            return await _specializationRepository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a Category in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            return await _categoryRepository.UpdateAsync(entity);
        }
        
        /// <summary>
        /// Updates a Specialization in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            return await _specializationRepository.UpdateAsync(entity);
        }
    }
}
