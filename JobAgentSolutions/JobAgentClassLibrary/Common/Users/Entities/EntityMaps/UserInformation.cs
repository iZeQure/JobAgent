using Dapper.Contrib.Extensions;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Entities.EntityMaps
{
    [Table("UserInformation")]
    public class UserInformation : IUser
    {
        public int Id => UserId;

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public Location Location => new() { Id = LocationId };

        public Role Role => new() { Id = RoleId };

        public List<Area> ConsultantAreas { get; set; }


        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int LocationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }

    }
}
