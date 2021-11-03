using Dapper;
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
                string proc = "[JA.spCreateArea]";
                var values = new
                {
                    @areaName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
                //using (var cmd = conn.CreateCommand())
                //{
                //    cmd.CommandText = 
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddRange(values);

                //    try
                //    {
                //        await conn.OpenAsync();

                //        entityId = (int)await cmd.ExecuteScalarAsync();
                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }
                //}
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }


        public async Task<List<IArea>> GetAllAsync()
        {
            List<IArea> areas = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetAreas]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var area = new Area
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };

                                areas.Add(area);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }

            return areas;
        }


        public async Task<IArea> GetByIdAsync(int id)
        {
            IArea area = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@areaId", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetAreaById]";
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
                                area = new Area()
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@areaId", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spRemoveArea]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        var execResult = (await cmd.ExecuteScalarAsync()).ToString();
                        entityId = int.Parse(execResult);
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@areaId", entity.Id),
                        new SqlParameter("@areaName", entity.Name)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateArea]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        var procResult = (await cmd.ExecuteScalarAsync()).ToString();
                        entityId = int.Parse(procResult);

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
