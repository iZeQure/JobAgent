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
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public SpecializationRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }

        public async Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@categoryId", entity.CategoryId),
                    new SqlParameter("@SpecializationName", entity.Name)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spCreateSpecialization]";
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

        public async Task<List<ISpecialization>> GetAllAsync()
        {
            List<ISpecialization> specializations = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetSpecializations]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                var specialization = new Specialization
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CategoryId = reader.GetInt32(2)
                                };

                                specializations.Add(specialization);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return specializations;
        }

        public async Task<ISpecialization> GetByIdAsync(int id)
        {
            ISpecialization specialization = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetSpecializationById]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@id", id);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                specialization = new Specialization
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CategoryId = reader.GetInt32(2)
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

            return specialization;
        }

        public async Task<bool> RemoveAsync(ISpecialization entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spRemoveSpecialization]";
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

        public async Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@id", entity.Id),
                    new SqlParameter("@specializationName", entity.Name),
                    new SqlParameter("@categoryId", entity.CategoryId)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateSpecialization]";
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
