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

        public async Task<ILocation> CreateAsync(ILocation entity)
        {
            return await _locationRepository.CreateAsync(entity);
        }

        public async Task<ILocation> GetByIdAsync(int id)
        {
            return await _locationRepository.GetByIdAsync(id);
        }

        public async Task<List<ILocation>> GetLocationsAsync()
        {
            return await _locationRepository.GetAllAsync();
        }

        public async Task<bool> RemoveAsync(ILocation entity)
        {
            return await _locationRepository.RemoveAsync(entity);
        }

        public async Task<ILocation> UpdateAsync(ILocation entity)
        {
            return await _locationRepository.UpdateAsync(entity);
        }
    }
}
