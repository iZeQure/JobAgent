using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class StaticSearchFilterService : IStaticSearchFilterService
    {
        private readonly IStaticSearchFilterRepository _repository;
        private readonly ILogService _logService;

        public StaticSearchFilterService(IStaticSearchFilterRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a StaticSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create StaticSearchFilter", nameof(CreateAsync), nameof(StaticSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all StaticSearchFilters in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IStaticSearchFilter>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get StaticSearchFilters", nameof(GetAllAsync), nameof(StaticSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific StaticSearchFilter from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get StaticSearchFilter by id", nameof(GetByIdAsync), nameof(StaticSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a StaticSearchFilter from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove StaticSearchFilter", nameof(RemoveAsync), nameof(StaticSearchFilterService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a StaticSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update StaticSearchFilter", nameof(UpdateAsync), nameof(StaticSearchFilterService), LogType.SERVICE);
                throw;
            }
        }
    }
}
