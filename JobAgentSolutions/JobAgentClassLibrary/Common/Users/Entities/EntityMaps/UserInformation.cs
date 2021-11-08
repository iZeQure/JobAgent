using Dapper.Contrib.Extensions;
using JobAgentClassLibrary.Common.Areas.Entities;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Users.Entities.EntityMaps
{
    [Table("UserInformation")]
    public class UserInformation : IUser
    {
        public int Id => UserId;

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public List<IArea> ConsultantAreas { get; set; }

        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int LocationId { get; set; }
        public int RoleId { get; set; }

    }
}
