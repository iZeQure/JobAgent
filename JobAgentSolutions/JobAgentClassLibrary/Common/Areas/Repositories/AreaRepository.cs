using Dapper;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Areas.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly AreaEntityFactory _factory;

        public AreaRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
            _factory = new AreaEntityFactory();
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

                var queryResult = await conn.QueryAsync<AreaInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IArea area = (IArea)_factory.CreateEntity(
                                nameof(Area),
                                result.Id, result.Name);

                        areas.Add(area);
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
                    area = (IArea)_factory.CreateEntity(
                                nameof(Area),
                                queryResult.Id, queryResult.Name);
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
                var values = new
                {
                    @areaId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }


        public async Task<IArea> UpdateAsync(IArea entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var proc = "[JA.spUpdateArea]";
                var values = new
                {
                    @areaId = entity.Id,
                    @areaName = entity.Name
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
