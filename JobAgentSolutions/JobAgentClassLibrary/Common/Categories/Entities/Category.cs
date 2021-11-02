using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ISpecialization> Specializations { get; set; }
    }
}
