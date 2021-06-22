using ObjectLibrary.Common;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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

        public async Task<int> CreateAsync(Role createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateRole];";
                SqlParameter[] parameters = new[]
                {
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

        public async Task<int> DeleteAsync(Role deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteRole];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@roleId", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetRoles];";

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

        public async Task<Role> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetRoleById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@roleId", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        Role role = new(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2));
                    }
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateAsync(Role updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateRole];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@roleId", updateEntity.Id),
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
