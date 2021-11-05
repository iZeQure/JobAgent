using Dapper.Contrib.Extensions;


namespace JobAgentClassLibrary.Common.Filters.Entities.EntityMaps
{
    [Table("FilterTypeInformation")]
    public class FilterTypeInformation : IFilterType
    {
        public int Id => FilterTypeId;
        
        public string Name => FilterTypeName;

        public string Description => FilterTypeDescription;


        [Key]
        public int FilterTypeId { get; set; }
        public string FilterTypeName { get; set; }
        public string FilterTypeDescription { get; set; }
    }
}
