using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Filters.Entities.EntityMaps
{
    [Table("StaticSearchFilterInformation")]
    public class StaticSearchFilterInformation : IStaticSearchFilter
    {
        public int Id => StaticSearchFilterId;

        public FilterType FilterType => new() { Id = FilterTypeId };

        public string Key => StaticSearchFilterKey;


        [Key]
        public int StaticSearchFilterId { get; set; }
        public int FilterTypeId { get; set; }
        public string StaticSearchFilterKey { get; set; }

    }
}
