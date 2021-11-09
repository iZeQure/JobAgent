using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Roles.Entities.EntityMaps
{
    [Table("RoleInformation")]
    public class RoleInformation : IRole
    {
        public int Id => RoleId;

        public string Name => RoleName;

        public string Description => RoleDescription;


        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
    }
}
