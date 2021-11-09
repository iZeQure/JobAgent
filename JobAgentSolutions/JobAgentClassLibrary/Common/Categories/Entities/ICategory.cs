using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public interface ICategory : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }
        public List<ISpecialization> Specializations { get; }
    }
}
