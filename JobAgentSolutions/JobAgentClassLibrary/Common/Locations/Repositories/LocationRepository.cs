﻿using Dapper;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Locations.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Locations.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Locations.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly LocationEntityFactory _factory;

        public LocationRepository(ISqlDbManager sqlDbManager, LocationEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<ILocation> CreateAsync(ILocation entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateLocation]";

                var values = new
                {
                    @LocationId = entity.Id,
                    @LocationName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<ILocation>> GetAllAsync()
        {
            List<ILocation> locations = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetLocations]";

                var queryResult = await conn.QueryAsync<LocationInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ILocation location = (ILocation)_factory.CreateEntity(
                                nameof(Location),
                                result.LocationId, result.LocationName);

                        locations.Add(location);
                    }
                }
            }

            return locations;
        }

        public async Task<ILocation> GetByIdAsync(int id)
        {
            ILocation location = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetLocationById]";
                var values = new
                {
                    @locationId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<LocationInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    location = (ILocation)_factory.CreateEntity(
                                nameof(Location),
                                queryResult.Id, queryResult.Name);
                }
            }

            return location;
        }

        public async Task<bool> RemoveAsync(ILocation entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveLocation]";
                var values = new
                {
                    @locationId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<ILocation> UpdateAsync(ILocation entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateLocation]";
                var values = new
                {
                    @locationId = entity.Id,
                    @locationName = entity.Name
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
