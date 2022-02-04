using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Roles.Repositories
{
    public interface IRoleRepository : IRepository<IRole, int>
    {
    }
}
