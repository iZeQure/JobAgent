using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts.Repositories
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public JobAdvertRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@vacantJobId", entity.Id),
                    new SqlParameter("@categoryId", entity.CategoryId),
                    new SqlParameter("@specializationid", entity.SpecializationId),
                    new SqlParameter("@jobAdvertTitle", entity.Title),
                    new SqlParameter("@jobAdvertSummary", entity.Summary),
                    new SqlParameter("@jobAdvertRegistrationDateTime", entity.RegistrationDateTime),
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateJobAdvert]";
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


        public async Task<List<IJobAdvert>> GetAllAsync()
        {
            List<IJobAdvert> jobAdverts = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetJobAdverts]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var jobAdvert = new JobAdvert
                                {
                                    Id = reader.GetInt32(0),
                                    VacantJobId = reader.GetInt32(0),
                                    CategoryId = reader.GetInt32(6),
                                    SpecializationId = reader.GetInt32(8),
                                    Title = reader.GetString(11),
                                    Summary = reader.GetString(12),
                                    RegistrationDateTime = reader.GetDateTime(13)

                                };

                                jobAdverts.Add(jobAdvert);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return jobAdverts;
        }

        public Task<List<JobAdvert>> GetAllUncategorized()
        {
            throw new NotImplementedException();
        }


        public async Task<IJobAdvert> GetByIdAsync(int id)
        {
            IJobAdvert jobAdvert = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetJobAdvertById]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                jobAdvert = new JobAdvert
                                {
                                    Id = reader.GetInt32(0),
                                    VacantJobId = reader.GetInt32(0),
                                    CategoryId = reader.GetInt32(6),
                                    SpecializationId = reader.GetInt32(8),
                                    Title = reader.GetString(11),
                                    Summary = reader.GetString(12),
                                    RegistrationDateTime = reader.GetDateTime(13)
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
            return jobAdvert;
        }


        public async Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countByCategory", SqlDbType = SqlDbType.Int };

                SqlParameter[] values = new[]
                {
                    new SqlParameter("@categoryId", id),
                    outputParameter
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetTotalJobAdvertCountByCategoryId]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);
                    await cmd.ExecuteNonQueryAsync();
                }

                bool parsedOutput = int.TryParse(values[1].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return count;
                }

                return 0;
            }
        }


        public async Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countByCategory", SqlDbType = SqlDbType.Int };

                SqlParameter[] values = new[]
                {
                    new SqlParameter("@specializationId", id),
                    outputParameter
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetTotalJobAdvertCountBySpecializationId]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);
                    await cmd.ExecuteNonQueryAsync();
                }

                bool parsedOutput = int.TryParse(values[1].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return count;
                }

                return 0;
            }
        }


        public async Task<int> GetJobAdvertCountByUncategorized()
        {
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countByCategory", SqlDbType = SqlDbType.Int };

                SqlParameter[] values = new[]
                {
                    outputParameter
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetTotalJobAdvertCountByNonCategorized]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);
                    await cmd.ExecuteNonQueryAsync();
                }

                bool parsedOutput = int.TryParse(values[1].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return count;
                }

                return 0;
            }
        }


        public async Task<bool> RemoveAsync(IJobAdvert entity)
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
                    cmd.CommandText = "[JA.spRemoveJobAdvert]";
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
                return true;
            }

            return false;
        }


        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@vacantJobId", entity.Id),
                    new SqlParameter("@categoryId", entity.CategoryId),
                    new SqlParameter("@specializationid", entity.SpecializationId),
                    new SqlParameter("@jobAdvertTitle", entity.Title),
                    new SqlParameter("@jobAdvertSummary", entity.Summary),
                    new SqlParameter("@jobAdvertRegistrationDateTime", entity.RegistrationDateTime)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateJobAdvert]";
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
