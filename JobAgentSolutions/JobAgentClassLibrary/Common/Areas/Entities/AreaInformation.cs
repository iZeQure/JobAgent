using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Areas.Entities
{
    [Table("AreaInformation")]
    public class AreaInformation : IArea
    {
        public string Name => AreaName;

        public int Id => AreaId;

        [Key]
        private int AreaId { get; set; }
        private string AreaName { get; set; }
    }
}
