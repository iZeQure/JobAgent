using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;

namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class User : IUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get { return $"{FirstName} {LastName}"; } }

        public string Email { get; set; }

        public Location Location { get; set; }

        public Role Role { get; set; }

        public int Id { get; set; }
    }
}
