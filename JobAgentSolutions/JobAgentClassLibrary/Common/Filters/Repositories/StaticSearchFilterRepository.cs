using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public class StaticSearchFilterRepository : IStaticSearchFilterRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public StaticSearchFilterRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@categoryId", entity),
                    new SqlParameter("@key", entity.Key)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateDynamicSearchFilter]";
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

        public async Task<List<IStaticSearchFilter>> GetAllAsync()
        {
            List<IStaticSearchFilter> staticSearchFilters = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetStaticSearchFilters]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var staticSearchFilter = new StaticSearchFilter
                                {
                                    Id = reader.GetInt32(0),
                                    Key = reader.GetString(2)

                                    //FORSÆT HERFRA I MORGEN JESPER!

                                };

                                staticSearchFilters.Add(staticSearchFilter);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return staticSearchFilters;
        }

        public Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }

        public Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            throw new NotImplementedException();
        }
    }
}
