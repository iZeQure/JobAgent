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
    public class DynamicSearchFilterRepository : IDynamicSearchFilterRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public DynamicSearchFilterRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }


        public async Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@categoryId", entity.CategoryId),
                    new SqlParameter("@specializationId", entity.SpecializationId),
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


        public async Task<List<IDynamicSearchFilter>> GetAllAsync()
        {
            List<IDynamicSearchFilter> dynamicSearchFilters = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetDynamicSearchFilters]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var company = new DynamicSearchFilter
                                {
                                    Id = reader.GetInt32(0),
                                    CategoryId = reader.GetInt32(1),
                                    SpecializationId = reader.GetInt32(2),
                                    Key = reader.GetString(3)

                                };

                                dynamicSearchFilters.Add(company);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return dynamicSearchFilters;
        }


        public async Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            DynamicSearchFilter dynamincSearchFilter = new();
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var values = new SqlParameter[]
                {
                        new SqlParameter("@id", id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetDynamicSearchFilterById]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                dynamincSearchFilter = new()
                                {
                                    Id = reader.GetInt32(0),
                                    CategoryId = reader.GetInt32(1),
                                    SpecializationId = reader.GetInt32(2),
                                    Key = reader.GetString(3)
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
            return dynamincSearchFilter;
        }


        public async Task<bool> RemoveAsync(IDynamicSearchFilter entity)
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
                    cmd.CommandText = "[JA.spRemoveDynamicSearchFilter]";
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


        public async Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@categoryId", entity.CategoryId),
                    new SqlParameter("@specializationId", entity.SpecializationId),
                    new SqlParameter("@key", entity.Key)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateDynamicSearchFilter]";
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
