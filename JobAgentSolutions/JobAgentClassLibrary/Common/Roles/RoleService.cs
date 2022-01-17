using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Roles.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogService _logService;

        public RoleService(IRoleRepository roleRepository, ILogService logService)
        {
            _roleRepository = roleRepository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new Role in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IRole> CreateAsync(IRole entity)
        {
            try
            {
                return await _roleRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create role", nameof(CreateAsync), nameof(RoleService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all Roles in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IRole>> GetRolesAsync()
        {
            try
            {
                return await _roleRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create roles", nameof(GetRolesAsync), nameof(RoleService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific Role from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IRole> GetRoleByIdAsync(int id)
        {
            try
            {
                return await _roleRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get role by id", nameof(GetRoleByIdAsync), nameof(RoleService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a Role from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IRole entity)
        {
            try
            {
                return await _roleRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove role", nameof(RemoveAsync), nameof(RoleService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a Role in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IRole> UpdateAsync(IRole entity)
        {
            try
            {
                return await _roleRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update role", nameof(UpdateAsync), nameof(RoleService), LogType.SERVICE);
                throw;
            }
        }
    }
}
