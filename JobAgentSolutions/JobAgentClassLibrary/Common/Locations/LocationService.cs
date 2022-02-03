using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Locations.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly ILogService _logService;

        public LocationService(ILocationRepository locationRepository, ILogService logService)
        {
            _locationRepository = locationRepository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new Location in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<ILocation> CreateAsync(ILocation entity)
        {
            try
            {
                return await _locationRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create location", nameof(CreateAsync), nameof(LocationService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific Location from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ILocation> GetByIdAsync(int id)
        {
            try
            {
                return await _locationRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get location by id", nameof(GetByIdAsync), nameof(LocationService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Returns all Locations in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ILocation>> GetLocationsAsync()
        {
            try
            {
                return await _locationRepository.GetAllSystemLogsAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get locations", nameof(GetLocationsAsync), nameof(LocationService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Removes a Location from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ILocation entity)
        {
            try
            {
                return await _locationRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove location", nameof(RemoveAsync), nameof(LocationService), LogType.SYSTEM);
                throw;
            }
        }

        /// <summary>
        /// Updates a Location in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ILocation> UpdateAsync(ILocation entity)
        {
            try
            {
                return await _locationRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {

                await _logService.LogError(ex, "Failed to update location", nameof(UpdateAsync), nameof(LocationService), LogType.SYSTEM);
                throw;
            }
        }
    }
}
