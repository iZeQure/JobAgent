using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Locations.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Locations
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        /// <summary>
        /// Creates a new Location in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<ILocation> CreateAsync(ILocation entity)
        {
            return await _locationRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a specific Location from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ILocation> GetByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Returns all Locations in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ILocation>> GetLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        /// <summary>
        /// Removes a Location from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ILocation entity)
        {
            return await _locationRepository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a Location in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ILocation> UpdateAsync(ILocation entity)
        {
            return await _locationRepository.UpdateAsync(entity);
        }
    }
}
