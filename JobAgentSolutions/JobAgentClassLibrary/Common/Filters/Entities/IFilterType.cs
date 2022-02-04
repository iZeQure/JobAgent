namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IFilterType : IFilter
    {
        public string Name { get; }
        public string Description { get; }
    }
}
