namespace JobAgentClassLibrary.Common.Filters.Entities
{
    /// <summary>
    /// Handles the data of a dynamic search filter.
    /// </summary>
    public class DynamicSearchFilter : IDynamicSearchFilter
    {
        public int CategoryId { get; set; }

        public int SpecializationId { get; set; }

        public string Key { get; set; }

        public int Id { get; set; }
    }
}
