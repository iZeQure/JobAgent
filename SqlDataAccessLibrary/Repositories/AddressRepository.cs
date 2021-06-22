﻿using ObjectLibrary.Common;
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
    public class AddressRepository : IAddressRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public AddressRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public async Task<int> CreateAsync(Address createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateAddress];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@jobAdvertVacantJobId", createEntity.Id),
                    new SqlParameter("@streetAddress", createEntity.StreetAddress),
                    new SqlParameter("@city", createEntity.City),
                    new SqlParameter("@country", createEntity.Country),
                    new SqlParameter("@postalCode", createEntity.PostalCode)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(Address deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteAddress];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@jobAdvertVacantJobId", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Address>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetAddresses];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<Address> addresses = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        addresses.Add(new Address(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)));
                    }

                    return await Task.FromResult(addresses);
                }

                return await Task.FromResult(Enumerable.Empty<Address>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Address> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetAddressById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@jobAdvertVacantJobId", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Address address = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        address = new Address(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4));
                    }

                    return await Task.FromResult(address);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> UpdateAsync(Address updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateAddress];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@jobAdvertVacantJobId", updateEntity.Id),
                    new SqlParameter("@streetAddress", updateEntity.StreetAddress),
                    new SqlParameter("@city", updateEntity.City),
                    new SqlParameter("@country", updateEntity.Country),
                    new SqlParameter("@postalCode", updateEntity.PostalCode)
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
