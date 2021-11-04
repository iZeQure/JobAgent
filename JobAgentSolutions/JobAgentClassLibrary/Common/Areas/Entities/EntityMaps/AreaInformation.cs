using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Areas.Entities.EntityMaps
{
    [Table("AreaInformation")]
    public class AreaInformation : IArea
    {
        public string Name => AreaName;

        public int Id => AreaId;

        [Key]
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }
}
