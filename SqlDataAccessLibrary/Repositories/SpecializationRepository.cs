using ObjectLibrary.Common;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public SpecializationRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a new Specialization
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(Specialization createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateSpecialization];";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@categoryId", createEntity.Category.Id),
                    new SqlParameter("@SpecializationName", createEntity.Name)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);


            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deletes a Specialization
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Specialization deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveSpecialization]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a collection of all Specializations
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Specialization>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {

                string cmdText = "EXEC [JA.spGetSpecializations]";
                List<Specialization> tempSpecializationList = new();

                SqlParameter[] parameters = new SqlParameter[] { };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        Specialization tempSpecialization = new(
                            id: reader.GetInt32(0),
                            category: new Category(reader.GetInt32(1), ""),
                            name: reader.GetString(2)
                            );

                        tempSpecializationList.Add(tempSpecialization);
                    }
                }

                return tempSpecializationList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets specialization from ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Specialization> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetSpecializationById]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Specialization tempSpecialization = null;
                    while (await reader.ReadAsync(cancellation))
                    {

                        tempSpecialization = new(
                        id: reader.GetInt32(0),
                        category: new Category(reader.GetInt32(1), ""),
                        name: reader.GetString(2)
                        );

                    }
                    return tempSpecialization;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a specialization
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Specialization updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateSpecialization]";

                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@id", updateEntity.Id),
                new SqlParameter("@specializationName", updateEntity.Name),
                new SqlParameter("@categoryId", updateEntity.Category.Id)
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
