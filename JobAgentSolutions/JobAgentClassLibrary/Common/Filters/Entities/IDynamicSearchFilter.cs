namespace JobAgentClassLibrary.Common.Filters.Entities
{
    /// <summary>
    /// Represents a generic dynamic search filter.
    /// </summary>
    public interface IDynamicSearchFilter : IFilter
    {
        public int CategoryId { get; }
        public int SpecializationId { get; }
        public string Key { get; }

    }
}
