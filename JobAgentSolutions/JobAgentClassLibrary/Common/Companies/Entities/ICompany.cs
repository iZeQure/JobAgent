using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Companies.Entities
{
    public interface ICompany : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }
    }
}
