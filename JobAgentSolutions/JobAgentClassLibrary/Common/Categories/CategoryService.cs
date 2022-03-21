using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
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
        private readonly ILogService _logService;

        public CategoryService(ICategoryRepository categoryRepository, ISpecializationRepository specializationRepository, ILogService logService)
        {
            _categoryRepository = categoryRepository;
            _specializationRepository = specializationRepository;
            _logService = logService;
        }

        /// <summary>   
        /// Creates a new Category in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved object</returns>
        public async Task<ICategory> CreateAsync(ICategory entity)
        {
            try
            {
                return await _categoryRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create category", nameof(CreateAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Creates a new Specialization in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>returns the saved object</returns>
        public async Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            try
            {
                return await _specializationRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create specialization", nameof(CreateAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all Categories in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ICategory>> GetCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get categories", nameof(GetCategoriesAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of a specific Category
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICategory> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get category by id", nameof(GetCategoryByIdAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of categories matched with specializations for the sidebarmenu
        /// </summary>
        /// <returns></returns>
        public async Task<List<ICategory>> GetMenuAsync()
        {
            try
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
                    if (c.Id is 0)
                    {
                        continue;
                    }

                    var matches = specializations.Where(s => s.CategoryId == c.Id);

                    if (!matches.Any())
                    {
                        continue;
                    }

                    c.AddRange(matches);
                }

                return categories;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get menu", nameof(GetMenuAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific Specialization from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ISpecialization> GetSpecializationByIdAsync(int id)
        {
            try
            {
                return await _specializationRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get specialization by id", nameof(GetSpecializationByIdAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all Specializations from the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ISpecialization>> GetSpecializationsAsync()
        {
            try
            {
                return await _specializationRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get specializations", nameof(GetSpecializationsAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Removes a Category from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ICategory entity)
        {
            try
            {
                return await _categoryRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove category", nameof(RemoveAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Removes a Specialization from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ISpecialization entity)
        {
            try
            {
                return await _specializationRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove specialization", nameof(RemoveAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Updates a Category in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            try
            {
                return await _categoryRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update category", nameof(UpdateAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Updates a Specialization in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            try
            {
                return await _specializationRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update specialization", nameof(UpdateAsync), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }

        public async Task<ICategory> GetCategoryWithSpecializationsById(int id)
        {
            try
            {
                ICategory category = await GetCategoryByIdAsync(id);

                foreach (ISpecialization specialization in await GetSpecializationsAsync())
                {
                    if (specialization.CategoryId == id)
                    {
                        category.Specializations.Add(specialization);
                    }
                }

                return category;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get category details", nameof(GetCategoryWithSpecializationsById), nameof(CategoryService), LogType.SYSTEM);
                throw;
            }
        }
    }
}
