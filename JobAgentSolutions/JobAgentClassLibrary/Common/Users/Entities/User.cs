using JobAgentClassLibrary.Common.Areas.Entities;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class User : IUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public string Email { get; set; }

        public int LocationId { get; set; }

        public int RoleId { get; set; }

        public int Id { get; set; }

        public List<IArea> ConsultantAreas { get; set; }
    }
}
