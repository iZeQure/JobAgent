﻿using Dapper;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities.EntityMaps;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs.Repositories
{
    public class VacantJobRepository : IVacantJobRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly VacantJobEntityFactory _factory;

        public VacantJobRepository(ISqlDbManager sqlDbManager, VacantJobEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        /// <summary>
        /// Creates a new VacantJob in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var proc = "[JA.spCreateVacantJob]";
                var values = new
                {
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        /// <summary>
        /// Returns a list of all VacantJobs in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IVacantJob>> GetAllAsync()
        {
            List<IVacantJob> vacantJobs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetVacantJobs]";

                var queryResult = await conn.QueryAsync<VacantJobInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IVacantJob vacantJob = (IVacantJob)_factory.CreateEntity(nameof(VacantJob), result.Id, result.CompanyId, result.URL);

                        vacantJobs.Add(vacantJob);
                    }
                }
            }

            return vacantJobs;
        }

        /// <summary>
        /// Returns a specific VacantJob from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            IVacantJob vacantJob = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetVacantJobById]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<VacantJobInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    vacantJob = (IVacantJob)_factory.CreateEntity(
                                nameof(VacantJob),
                                queryResult.Id, queryResult.CompanyId, queryResult.URL);
                }
            }

            return vacantJob;
        }

        /// <summary>
        /// Removes a VacantJob from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveVacantJob]";
                var values = new
                {
                    @vacantJobId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        /// <summary>
        /// Updates a VacantJob in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateVacantJob]";
                var values = new
                {
                    @vacantJobId = entity.Id,
                    @companyId = entity.CompanyId,
                    @url = entity.URL
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
