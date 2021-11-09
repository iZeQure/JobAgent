using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Categories.Entities.EntityMaps
{
    [Table("SpecializationInformation")]
    public class SpecializationInformation : ISpecialization
    {
        public string Name => SpecializationName;

        public int Id => SpecializationId;

        [Key]
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public int CategoryId { get; set; }
    }
}
