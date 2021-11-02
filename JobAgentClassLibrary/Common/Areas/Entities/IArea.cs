using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Areas.Entities
{
    public interface IArea : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }

    }
}
