using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Users.Entities
{
    public interface IUser : IAggregateRoot, IEntity<int>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName { get; }
        public string Email { get; }
        public Location Location { get; }
        public Role Role { get; }
        public List<Area> ConsultantAreas { get; }
    }
}
