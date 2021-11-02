using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IFilter : IAggregateRoot, IEntity<int>
    {
    }
}
