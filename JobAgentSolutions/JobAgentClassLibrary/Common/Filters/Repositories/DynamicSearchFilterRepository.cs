using Dapper;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Filters.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public class DynamicSearchFilterRepository : IDynamicSearchFilterRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly DynamicSearchFilterEntityFactory _factory;


        public DynamicSearchFilterRepository(ISqlDbManager sqlDbManager, DynamicSearchFilterEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }


        public async Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateDynamicSearchFilter]";

                var values = new
                {
                    @categoryId = entity.CategoryId,
                    @specializationId = entity.SpecializationId,
                    @key = entity.Key
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }


        public async Task<List<IDynamicSearchFilter>> GetAllAsync()
        {
            List<IDynamicSearchFilter> DynamicSearchFilters = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetDynamicFilterKeys]";

                var queryResult = await conn.QueryAsync<DynamicSearchFilterInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IDynamicSearchFilter dynamicSearchFilter = (IDynamicSearchFilter)_factory.CreateEntity(
                                nameof(DynamicSearchFilter),
                                result.DynamicSearchFilterId, result.SpecializationId, result.CategoryId, result.DynamicSearchFilterKey);

                        DynamicSearchFilters.Add(dynamicSearchFilter);
                    }
                }
            }

            return DynamicSearchFilters;
        }


        public async Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            IDynamicSearchFilter dynamicSearchFilter = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetDynamicSearchFilterById]";
                var values = new
                {
                    @id = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<DynamicSearchFilterInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    dynamicSearchFilter = (IDynamicSearchFilter)_factory.CreateEntity(
                                nameof(DynamicSearchFilter),
                                queryResult.DynamicSearchFilterId, queryResult.SpecializationId, queryResult.CategoryId, queryResult.DynamicSearchFilterKey);
                }
            }

            return dynamicSearchFilter;
        }


        public async Task<bool> RemoveAsync(IDynamicSearchFilter entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveDynamicSearchFilter]";
                var values = new
                {
                    @id = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }


        public async Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateDynamicSearchFilter]";
                var values = new
                {
                    @id = entity.Id,
                    @specializationId = entity.SpecializationId,
                    @categoryId = entity.CategoryId,
                    @key = entity.Key
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
