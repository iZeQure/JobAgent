using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Locations.Repositories
{
    public interface ILocationRepository : IRepository<ILocation, int>
    {
    }
}
