using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Roles.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IRole> CreateAsync(IRole entity)
        {
            return await _roleRepository.CreateAsync(entity);
        }

        public async Task<List<IRole>> GetRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        public async Task<IRole> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IRole entity)
        {
            return await _roleRepository.RemoveAsync(entity);
        }

        public async Task<IRole> UpdateAsync(IRole entity)
        {
            return await _roleRepository.UpdateAsync(entity);
        }
    }
}
