using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public AreaRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<IArea> CreateAsync(IArea entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@name", entity.Name)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateArea]";
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


        public async Task<List<IArea>> GetAllAsync()
        {
            List<IArea> categories = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetAreas]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var category = new Area
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };

                                categories.Add(category);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }

            return categories;
        }


        public async Task<IArea> GetByIdAsync(int id)
        {
            Area area = new Area();
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetAreaById]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                area = new()
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
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

            return area;
        }


        public async Task<bool> RemoveAsync(IArea entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spDeleteArea]";
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


        public async Task<IArea> UpdateAsync(IArea entity)
        {

            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", entity.Id),
                        new SqlParameter("@name", entity.Name)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateArea]";
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
