using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Loggings.Entities
{
    public interface ILog : IAggregateRoot, IEntity<int>
    {
    }
}
