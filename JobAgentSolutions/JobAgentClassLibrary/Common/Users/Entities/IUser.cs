using JobAgentClassLibrary.Common.Areas.Entities;
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
        public int LocationId { get; }
        public int RoleId { get; }
        public List<IArea> ConsultantAreas { get; }
    }
}
