namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public class StaticSearchFilter : IStaticSearchFilter
    {
        public FilterType FilterType { get; set; }

        public string Key { get; set; }

        public int Id { get; set; }
    }
}
