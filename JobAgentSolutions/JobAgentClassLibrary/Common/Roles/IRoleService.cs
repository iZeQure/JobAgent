using JobAgentClassLibrary.Common.Roles.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Roles
{
    public interface IRoleService
    {
        Task<IRole> CreateAsync(IRole entity);
        Task<List<IRole>> GetRolesAsync();
        Task<IRole> GetRoleByIdAsync(int id);
        Task<bool> RemoveAsync(IRole entity);
        Task<IRole> UpdateAsync(IRole entity);
    }
}
