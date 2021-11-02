namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IStaticSearchFilter : IFilter
    {
        public FilterType FilterType { get; }
        public string Key { get; }
    }
}
