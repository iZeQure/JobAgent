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

        /// <summary>
        /// Creates a new Role in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IRole> CreateAsync(IRole entity)
        {
            return await _roleRepository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all Roles in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IRole>> GetRolesAsync()
        {
            return await _roleRepository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific Role from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IRole> GetRoleByIdAsync(int id)
        {
            return await _roleRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a Role from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IRole entity)
        {
            return await _roleRepository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a Role in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IRole> UpdateAsync(IRole entity)
        {
            return await _roleRepository.UpdateAsync(entity);
        }
    }
}
