using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Locations.Entities.EntityMaps
{
    [Table("LocationInformation")]
    public class LocationInformation : ILocation
    {
        public int Id => LocationId;

        public string Name => LocationName;


        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
    }
}
