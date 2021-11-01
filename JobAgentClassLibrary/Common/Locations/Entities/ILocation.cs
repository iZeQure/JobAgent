using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Locations.Entities
{
    public interface ILocation : IAggregateRoot, IEntity<int>
    {
    }
}
