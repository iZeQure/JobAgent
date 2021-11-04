using Dapper;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs.Repositories
{
    public class VacantJobRepository : IVacantJobRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public VacantJobRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                string proc = "[JA.spCreateVacantJob]";

                var values = new
                {
                    @id = entity.Id,
                    @companyId = entity.CompanyId,
                    @vacantJobUrl = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<IVacantJob>> GetAllAsync()
        {
            List<IVacantJob> vacantJobs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetVacantJobs]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var vacantJob = new VacantJob
                                {
                                    Id = reader.GetInt32(0),
                                    URL = reader.GetString(1),
                                    CompanyId = reader.GetInt32(2)
                                };

                                vacantJobs.Add(vacantJob);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return vacantJobs;
        }

        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            IVacantJob vacantJob = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetVacantJobById]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                vacantJob = new VacantJob()
                                {
                                    Id = reader.GetInt32(0),
                                    URL = reader.GetString(1),
                                    CompanyId = reader.GetInt32(2)
                                };

                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return vacantJob;
        }

        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spDeleteVacantJob]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        entityId = int.Parse((await cmd.ExecuteScalarAsync()).ToString());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (entityId != 0)
            {
                return true;
            }

            return false;
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@companyId", entity.CompanyId),
                    new SqlParameter("@vacantJobUrl", entity.URL)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateVacantJob]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        entityId = int.Parse((await cmd.ExecuteScalarAsync()).ToString());
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (entityId >= 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
