using ObjectLibrary.Common;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public LocationRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public async Task<int> CreateAsync(Location createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateLocation];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@locationName", createEntity.Name)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> DeleteAsync(Location deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveLocation];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@locationId", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Location>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetLocations];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);
                List<Location> locations = new();

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        locations.Add(new Location(
                            reader.GetInt32(0),
                            reader.GetString(1)));
                    }

                    return await Task.FromResult(locations);
                }

                return await Task.FromResult(Enumerable.Empty<Location>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Location> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetLocationById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@locationId", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Location location = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        location = new Location(
                            reader.GetInt32(0),
                            reader.GetString(1));
                    }

                    return await Task.FromResult(location);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateAsync(Location updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateLocation];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@locationId", updateEntity.Id),
                    new SqlParameter("@locationName", updateEntity.Name)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
