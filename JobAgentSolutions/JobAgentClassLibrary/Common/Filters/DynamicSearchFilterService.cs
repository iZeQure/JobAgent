using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class DynamicSearchFilterService : IDynamicSearchFilterService
    {
        private readonly IDynamicSearchFilterRepository _repository;
        private readonly ILogService _logService;

        public DynamicSearchFilterService(IDynamicSearchFilterRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new DynamicSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create DynamicSearchFilter", nameof(CreateAsync), nameof(DynamicSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all DynamicSearchFilters in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDynamicSearchFilter>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllSystemLogsAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get DynamicSeachFilters", nameof(GetAllAsync), nameof(DynamicSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific DynamicSearchFilters from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get DynamicSeachFilter by id", nameof(GetByIdAsync), nameof(DynamicSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a DynamicSearchFilters from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IDynamicSearchFilter entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove DynamicSeachFilter", nameof(RemoveAsync), nameof(DynamicSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a DynamicSearchFilters in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update DynamicSeachFilter", nameof(UpdateAsync), nameof(DynamicSearchFilterService), LogType.SERVICE);
                throw;
            }
        }
    }
}
