using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public CategoryRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }

        public async Task<ICategory> CreateAsync(ICategory entity)
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
                    cmd.CommandText = "[JA.spCreateCategory]";
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
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<ICategory>> GetAllAsync()
        {
            List<ICategory> categories = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetCategories]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await conn.OpenAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var category = new Category
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

        public async Task<ICategory> GetByIdAsync(int id)
        {
            ICategory category = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetCategoryById]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                category = new Category
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

            return category;
        }

        public async Task<bool> RemoveAsync(ICategory entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spDeleteCategory]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        isDeleted = (await cmd.ExecuteNonQueryAsync()) == 1;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return isDeleted;
        }

        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@name", entity.Name)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateCategory]";
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
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
