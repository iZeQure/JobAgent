using ObjectLibrary.Common;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public RoleRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a new Role
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(Role createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spCreateRole";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@roleName", createEntity.Name),
                    new SqlParameter("@roleDescription", createEntity.Description)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deletes a Role
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Role deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spDeleteRole";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a collection with all Roles
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetRoles";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);
                List<Role> roles = new();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        roles.Add(new Role(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2)));
                    }

                    return await Task.FromResult(roles);
                }

                return await Task.FromResult(Enumerable.Empty<Role>());
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// Returns a specific role by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Role> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetRoleById";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Role role = null;
                    while (await reader.ReadAsync(cancellation))
                    {
                        role = new(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2));
                    }
                    return role;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a Role
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Role updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spUpdateRole";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@roleName", updateEntity.Name),
                    new SqlParameter("@roleDescription", updateEntity.Description)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
