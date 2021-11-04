using Dapper;
using Dapper.Contrib.Extensions;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
                await conn.OpenAsync();
                string proc = "[JA.spCreateArea]";
                var values = new
                {
                    @areaName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
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
                string proc = "[JA.spGetAreas]";
                //string cmdTxt = "SELECT * FROM [AreaInformation]";

                var queryResult = await conn.QueryAsync<AreaInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        areas.Add(new Area
                        {
                            Id = result.Id,
                            Name = result.Name
                        });
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
                var proc = "[JA.spGetAreaById]";
                var values = new
                {
                    @areaId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<AreaInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    area = new Area
                    {
                        Id = queryResult.Id,
                        Name = queryResult.Name
                    };
                }
            }

            return area;
        }


        public async Task<bool> RemoveAsync(IArea entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var proc = "[JA.spRemoveArea]";
                var values = new AreaInformation
                {
                    AreaId = entity.Id,
                    AreaName = entity.Name
                };

                isDeleted = await conn.DeleteAsync(values);
            }

            return isDeleted;
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
