using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public interface ICategory : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }

        public List<ISpecialization> Specializations { get; }

        /// <summary>
        /// Adds a range of elements to <see cref="Specializations"/>.
        /// </summary>
        /// <param name="elements">A range of elements added to the collection.</param>
        public void AddRange(IEnumerable<ISpecialization> elements);
    }
}
