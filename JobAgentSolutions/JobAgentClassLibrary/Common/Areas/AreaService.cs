using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _repository;
        private readonly ILogService _logService;

        public AreaService(IAreaRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Saves a new Area in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved object</returns>
        public async Task<IArea> CreateAsync(IArea entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create area", nameof(CreateAsync), nameof(AreaService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all areas
        /// </summary>
        /// <returns></returns>
        public async Task<List<IArea>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get areas", nameof(GetAllAsync), nameof(AreaService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific area on it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IArea> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get area by id", nameof(GetByIdAsync), nameof(AreaService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Removes a specific area from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IArea entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove area", nameof(RemoveAsync), nameof(AreaService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Updates a specific area in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IArea> UpdateAsync(IArea entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update area", nameof(UpdateAsync), nameof(AreaService), LogType.SYSTEM);
                throw;
            }
        }
    }
}
