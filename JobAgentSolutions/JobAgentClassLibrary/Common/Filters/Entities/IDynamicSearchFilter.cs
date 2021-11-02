namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IDynamicSearchFilter : IFilter
    {
        public int CategoryId { get; }
        public int SpecializationId { get; }
        public string Key { get; }

    }
}
