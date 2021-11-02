namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public class FilterType : IFilter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
