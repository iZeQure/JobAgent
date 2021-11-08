using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public class DbLogRepository : ILoggingRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public DbLogRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<ILog> CreateAsync(ILog entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@logSeverityId", entity.LogSeverity),
                    new SqlParameter("@createdDateTime", entity.CreatedDateTime),
                    new SqlParameter("@createdBy", entity.CreatedBy),
                    new SqlParameter("@action", entity.Action),
                    new SqlParameter("@message", entity.Message)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateLog]";
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

        public async Task<List<ILog>> GetAllAsync()
        {
            List<ILog> logs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetLogs]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var log = new DbLog
                                {
                                    Id = reader.GetInt32(0),
                                    LogSeverity = (LogSeverity)reader.GetInt32(1),
                                    CreatedDateTime = reader.GetDateTime(2),
                                    CreatedBy = reader.GetString(3),
                                    Action = reader.GetString(4),
                                    Message = reader.GetString(5)

                                };
                                logs.Add(log);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return logs;
        }

        public async Task<ILog> GetByIdAsync(int id)
        {
            ILog log = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetLogById]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                log = new DbLog()
                                {
                                    Id = reader.GetInt32(0),
                                    LogSeverity = (LogSeverity)reader.GetInt32(1),
                                    CreatedDateTime = reader.GetDateTime(2),
                                    CreatedBy = reader.GetString(3),
                                    Action = reader.GetString(4),
                                    Message = reader.GetString(5)
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

            return log;
        }

        public async Task<bool> RemoveAsync(ILog entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spDeleteLog]";
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

        public async Task<ILog> UpdateAsync(ILog entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var values = new SqlParameter[]
                {
                   new SqlParameter("@id", entity.Id),
                    new SqlParameter("@logSeverityId", entity.LogSeverity),
                    new SqlParameter("@createdDateTime", entity.CreatedDateTime),
                    new SqlParameter("@createdBy", entity.CreatedBy),
                    new SqlParameter("@action", entity.Action),
                    new SqlParameter("@message", entity.Message)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateLog]";
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
