using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public interface ISpecialization : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }
    }
}
