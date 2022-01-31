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
    public class FilterTypeRepository : IFilterTypeRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly FilterTypeEntityFactory _factory;

        public FilterTypeRepository(ISqlDbManager sqlDbManager, FilterTypeEntityFactory factory)
        {
            this._sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<IFilterType> CreateAsync(IFilterType entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateFilterType]";

                var values = new
                {
                    @name = entity.Name,
                    @description = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<IFilterType>> GetAllSystemLogsAsync()
        {
            List<IFilterType> FilterTypes = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetFilterTypes]";

                var queryResult = await conn.QueryAsync<FilterTypeInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IFilterType filterType = (IFilterType)_factory.CreateEntity(
                                nameof(FilterType),
                                result.FilterTypeId, result.FilterTypeName, result.FilterTypeDescription);

                        FilterTypes.Add(filterType);
                    }
                }
            }

            return FilterTypes;
        }

        public async Task<IFilterType> GetByIdAsync(int id)
        {
            IFilterType filterType = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetFilterTypeById]";
                var values = new
                {
                    @id = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<DynamicSearchFilterInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    filterType = (IFilterType)_factory.CreateEntity(
                                nameof(FilterType),
                                queryResult.DynamicSearchFilterId, queryResult.SpecializationId, queryResult.CategoryId, queryResult.DynamicSearchFilterKey);
                }
            }

            return filterType;
        }

        public async Task<bool> RemoveAsync(IFilterType entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveFilterType]";
                var values = new
                {
                    @id = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IFilterType> UpdateAsync(IFilterType entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateFilterType]";
                var values = new
                {
                    @id = entity.Id,
                    @name = entity.Name,
                    @description = entity.Description
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
