using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public class Category : ICategory
    {
        private readonly List<ISpecialization> _specializations = new();

        public int Id { get; set; }

        public string Name { get; set; }

        public List<ISpecialization> Specializations => _specializations;

        public void AddRange(IEnumerable<ISpecialization> elements) => _specializations.AddRange(elements);
    }
}
