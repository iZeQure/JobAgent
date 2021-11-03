using System.ComponentModel.DataAnnotations.Schema;

namespace JobAgentClassLibrary.Common.Areas.Entities
{
    public class Area : IArea
    {
        [Column("AreaId")]
        public int Id { get; set; }

        [Column("AreaName")]
        public string Name { get; set; }
    }
}
