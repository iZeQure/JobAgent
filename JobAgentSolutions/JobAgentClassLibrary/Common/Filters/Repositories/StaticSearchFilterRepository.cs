using Dapper;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Filters.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public class StaticSearchFilterRepository : IStaticSearchFilterRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly StaticSearchFilterEntityFactory _factory;


        public StaticSearchFilterRepository(ISqlDbManager sqlDbManager, StaticSearchFilterEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;

        }


        public async Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                string proc = "[JA.spCreateStaticSearchFilter]";

                var values = new
                {
                    @id = entity.Id,
                    @categoryId = entity.FilterType.Id,
                    @key = entity.Key
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<IStaticSearchFilter>> GetAllAsync()
        {
            List<IStaticSearchFilter> staticSearchFilters = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                string proc = "[JA.spGetStaticSearchFilters]";

                var queryResult = await conn.QueryAsync<StaticSearchFilterInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IStaticSearchFilter dynamicSearchFilter = (IStaticSearchFilter)_factory.CreateEntity(
                                nameof(StaticSearchFilter),
                                result.StaticSearchFilterId, result.FilterTypeId, result.StaticSearchFilterKey);

                        staticSearchFilters.Add(dynamicSearchFilter);
                    }
                }
            }

            return staticSearchFilters;
        }

        public async Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            IStaticSearchFilter staticSearchFilter = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var proc = "[JA.spGetStaticSearchFilterById]";
                var values = new
                {
                    @id = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<StaticSearchFilterInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    staticSearchFilter = (IStaticSearchFilter)_factory.CreateEntity(
                                nameof(StaticSearchFilter),
                                queryResult.StaticSearchFilterId, queryResult.FilterTypeId, queryResult.StaticSearchFilterKey);
                }
            }

            return staticSearchFilter;
        }

        public async Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var proc = "[JA.spRemoveStaticSearchFilter]";
                var values = new
                {
                    @id = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var proc = "[JA.spUpdateStaticSearchFilter]";
                var values = new
                {
                    @id = entity.Id,
                    @categoryId = entity.FilterType.Id,
                    @key = entity.Key
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
