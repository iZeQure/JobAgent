namespace JobAgentClassLibrary.Common.Areas.Entities
{
    public class AreaInformation : IArea
    {
        public int AreaId { get; set; }

        public string AreaName { get; set; }

        public int Id { get { return AreaId; } }

        public string Name { get { return AreaName; } }

    }
}
