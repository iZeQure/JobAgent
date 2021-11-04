using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Filters.Entities
{
    /// <summary>
    /// Represents a generic filter, used to identify a filter entity.
    /// </summary>
    public interface IFilter : IAggregateRoot, IEntity<int> { }
}
