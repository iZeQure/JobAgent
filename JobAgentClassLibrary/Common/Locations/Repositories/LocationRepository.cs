using JobAgentClassLibrary.Common.Locations.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Locations.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        public Task<ILocation> CreateAsync(ILocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ILocation>> GetAllAsync(ILocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<ILocation> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ILocation entity)
        {
            throw new NotImplementedException();
        }

        public Task<ILocation> UpdateAsync(ILocation entity)
        {
            throw new NotImplementedException();
        }
    }
}
