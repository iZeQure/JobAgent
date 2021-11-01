using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Users.Entities
{
    public interface IUser : IAggregateRoot, IEntity<int>
    {
    }
}
