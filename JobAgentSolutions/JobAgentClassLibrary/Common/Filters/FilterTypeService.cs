using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class FilterTypeService : IFilterTypeService
    {
        private readonly IFilterTypeRepository _repository;
        private readonly ILogService _logService;

        public FilterTypeService(IFilterTypeRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new FilterType in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IFilterType> CreateAsync(IFilterType entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create FilterType", nameof(CreateAsync), nameof(FilterTypeService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all FilterTypes in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IFilterType>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get FilterTypes", nameof(GetAllAsync), nameof(FilterTypeService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific FilterType from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IFilterType> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get FilterType by id", nameof(GetByIdAsync), nameof(FilterTypeService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Removes a FilterType from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IFilterType entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove FilterType", nameof(RemoveAsync), nameof(FilterTypeService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Updates a FilterType in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IFilterType> UpdateAsync(IFilterType entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update FilterType", nameof(UpdateAsync), nameof(FilterTypeService), LogType.SYSTEM);
                throw;
            }
        }
    }
}
