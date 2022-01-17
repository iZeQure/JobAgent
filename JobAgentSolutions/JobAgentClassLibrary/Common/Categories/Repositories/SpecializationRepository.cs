using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using JobAgentClassLibrary.Common.Categories.Factory;
using JobAgentClassLibrary.Common.Categories.Entities.EntityMaps;
using System;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly CategoryEntityFactory _factory;

        public SpecializationRepository(ISqlDbManager sqlDbManager, CategoryEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<ISpecialization> CreateAsync(ISpecialization entity)
        {
            if (string.IsNullOrEmpty(entity.Name))
            {
                throw new ArgumentNullException(nameof(entity), "Specialization name is Null or Empty.");
            }

            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateSpecialization]";
                var values = new
                {
                    @specializationName = entity.Name,
                    @categoryId = entity.CategoryId
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId > 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<ISpecialization>> GetAllAsync()
        {
            List<ISpecialization> specializations = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetSpecializations]";

                var queryResults = await conn.QueryAsync<SpecializationInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResults is not null)
                {
                    foreach (var result in queryResults)
                    {
                        ISpecialization specialization = (ISpecialization)_factory.CreateEntity(nameof(Specialization), result.Id, result.CategoryId, result.Name);

                        specializations.Add(specialization);
                    }
                }
            }

            return specializations;
        }

        public async Task<ISpecialization> GetByIdAsync(int id)
        {
            ISpecialization specialization = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetSpecializationById]";
                var values = new
                {
                    @specializationId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<SpecializationInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    specialization = (ISpecialization)_factory.CreateEntity(nameof(Specialization), queryResult.Id, queryResult.CategoryId, queryResult.Name);
                }
            }

            return specialization;
        }

        public async Task<bool> RemoveAsync(ISpecialization entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveSpecialization]";
                var values = new
                {
                    @specializationId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<ISpecialization> UpdateAsync(ISpecialization entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateSpecialization]";
                var values = new
                {
                    @specializationId = entity.Id,
                    @specializationName = entity.Name,
                    @categoryId = entity.CategoryId
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
