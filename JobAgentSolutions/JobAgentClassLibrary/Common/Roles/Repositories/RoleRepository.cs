using Dapper;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Roles.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Roles.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Roles.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly RoleEntityFactory _factory;

        public RoleRepository(ISqlDbManager sqlDbManager, RoleEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<IRole> CreateAsync(IRole entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateRole]";

                var values = new
                {
                    @roleId = entity.Id,
                    @roleName = entity.Name,
                    @roleDescription = entity.Description
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<IRole>> GetAllAsync()
        {
            List<IRole> roles = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetRoles]";

                var queryResult = await conn.QueryAsync<RoleInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IRole role = (IRole)_factory.CreateEntity(
                                nameof(Role),
                                result.Id, result.Name, result.Description);

                        roles.Add(role);
                    }
                }
            }

            return roles;
        }

        public async Task<IRole> GetByIdAsync(int id)
        {
            IRole role = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetRoleById]";
                var values = new
                {
                    @roleId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<RoleInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    role = (IRole)_factory.CreateEntity(
                                nameof(Role),
                                queryResult.Id, queryResult.Name, queryResult.Description);
                }
            }

            return role;
        }

        public async Task<bool> RemoveAsync(IRole entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spDeleteRole]";
                var values = new
                {
                    @roleId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IRole> UpdateAsync(IRole entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateRole]";
                var values = new
                {
                    @roleId = entity.Id,
                    @roleName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
