using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages.Repositories
{
    public class JobPageRepository : IJobPageRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public JobPageRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }

        public async Task<IJobPage> CreateAsync(IJobPage entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@jobPageId", entity.Id),
                    new SqlParameter("@companyId", entity.CompanyId),
                    new SqlParameter("@jobPageUrl", entity.URL)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateJobPage]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        entityId = (int)await cmd.ExecuteScalarAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<IJobPage>> GetAllAsync()
        {
            List<IJobPage> jobPages = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetJobPages]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var jobPage = new JobPage
                                {
                                    Id = reader.GetInt32(0),
                                    URL = reader.GetString(1),
                                    CompanyId = reader.GetInt32(2)
                                };

                                jobPages.Add(jobPage);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return jobPages;
        }

        public async Task<IJobPage> GetByIdAsync(int id)
        {
            IJobPage jobPage = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetJobPageById]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@jobPageId", id);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                jobPage = new JobPage
                                {
                                    Id = reader.GetInt32(0),
                                    CompanyId = reader.GetInt32(1),
                                    URL = reader.GetString(2)
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

            return jobPage;
        }

        public async Task<bool> RemoveAsync(IJobPage entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@jobPageId", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spDeleteJobPage]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        isDeleted = (await cmd.ExecuteNonQueryAsync()) == 1;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return isDeleted;
        }

        public async Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@jobPageId", entity.Id),
                    new SqlParameter("@companyId", entity.CompanyId),
                    new SqlParameter("@jobPageUrl", entity.URL)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateJobPage]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        entityId = (int)await cmd.ExecuteScalarAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
