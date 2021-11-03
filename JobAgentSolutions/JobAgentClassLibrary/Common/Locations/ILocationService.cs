using JobAgentClassLibrary.Common.Locations.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Locations
{
    public interface ILocationService
    {
        Task<ILocation> CreateAsync(ILocation entity);
        Task<List<ILocation>> GetLocationsAsync();
        Task<ILocation> GetByIdAsync(int id);
        Task<bool> RemoveAsync(ILocation entity);
        Task<ILocation> UpdateAsync(ILocation entity);
    }
}
