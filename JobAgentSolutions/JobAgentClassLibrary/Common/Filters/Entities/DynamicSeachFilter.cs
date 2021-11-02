namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public class DynamicSeachFilter : IDynamicSearchFilter
    {
        public int CategoryId { get; set; }

        public int SpecializationId { get; set; }

        public string Key { get; set; }

        public int Id { get; set; }
    }
}
