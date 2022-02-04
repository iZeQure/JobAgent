using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Filters.Entities.EntityMaps
{
    [Table("DynamicSearchFilterInformation")]
    public class DynamicSearchFilterInformation : IDynamicSearchFilter
    {
        public int Id => DynamicSearchFilterId;

        public int CategoryId { get; set; }

        public int SpecializationId { get; set; }

        public string Key => DynamicSearchFilterKey;


        [Key]
        public int DynamicSearchFilterId { get; set; }
        public string DynamicSearchFilterKey { get; set; }

    }
}
