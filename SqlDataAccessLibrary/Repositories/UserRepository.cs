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
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public UserRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Check to Authenticate a user login
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<bool> AuthenticateUserLoginAsync(IUser user, CancellationToken cancellation)
        {
            try
            {
                if (user == null)
                {
                    throw new NullReferenceException("Couldn't authenticate user login. User is null.");
                }

                string cmdText = "[JA.spValidateUserLogin]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@returnResult", SqlDbType = SqlDbType.Bit };

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userEmail", user.GetEmail),
                    new SqlParameter("@userPassword", user.GetPassword),
                    outputParameter
                };

                await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                bool output = Convert.ToBoolean(parameters[2].Value);

                return await Task.FromResult(output);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Checks if user exists in database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserExistsAsync(IUser user, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spValidateUserExists]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@returnResult", SqlDbType = SqlDbType.Bit };

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userEmail", user.GetEmail),
                    outputParameter
                };

                await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                bool output = Convert.ToBoolean(parameters[1].Value);

                return await Task.FromResult(output);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Creates new IUser
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(IUser createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spCreateUser]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@roleId", createEntity.GetRole.Id),
                    new SqlParameter("@locationId", createEntity.GetLocation.Id),
                    new SqlParameter("@firstName", createEntity.GetFirstName),
                    new SqlParameter("@lastName", createEntity.GetLastName),
                    new SqlParameter("@email", createEntity.GetEmail),
                    new SqlParameter("@password", createEntity.GetPassword),
                    new SqlParameter("@salt", createEntity.GetSalt),
                    new SqlParameter("@accessToken", createEntity.GetAccessToken)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Delete User
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(IUser deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spRemoveUser]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", deleteEntity.GetUserId)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }

        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<IUser>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetUsers]";

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<IUser> tempUserList = new();
                    while (await reader.ReadAsync(cancellation))
                    {
                        string delimiterAreas = await GetColumnDataOrEmptyStringAsync(reader, 6, cancellation);

                        IUser user = await GetUserDataFromReaderAsync(reader, cancellation);

                        tempUserList.Add(user);
                    }

                    return await Task.FromResult(tempUserList);
                }

                return await Task.FromResult(Enumerable.Empty<IUser>());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IUser> GetByEmailAsync(string email, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetUserByEmail]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@email", email)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    IUser user = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        user = await GetUserDataFromReaderAsync(reader, cancellation);
                    }

                    return await Task.FromResult(user);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> GetSaltByEmailAddressAsync(string email, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetUserSaltByEmail]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userEmail", email)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    var salt = string.Empty;

                    while (await reader.ReadAsync(cancellation))
                    {
                        salt = reader.GetString(0);
                    }

                    return await Task.FromResult(salt);
                }

                return await Task.FromResult(string.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets user by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IUser> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetUserById]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userAccessToken", id),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    IUser user = null;
                    while (await reader.ReadAsync(cancellation))
                    {
                        user = await GetUserDataFromReaderAsync(reader, cancellation);
                    }
                    return await Task.FromResult(user);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets user by Access Token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IUser> GetUserByAccessTokenAsync(string accessToken, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetUserByAccessToken]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userAccessToken", accessToken),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    IUser user = null;
                    while (await reader.ReadAsync(cancellation))
                    {
                        user = await GetUserDataFromReaderAsync(reader, cancellation);
                    }
                    return await Task.FromResult(user);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }        

        public async Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGrantUserArea]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", user.GetUserId),
                    new SqlParameter("@areaId", areaId)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> RemoveAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spRemoveUserArea]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", user.GetUserId),
                    new SqlParameter("@areaId", areaId)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(IUser updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spUpdateUser]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", updateEntity.GetUserId),
                    new SqlParameter("@roleId", updateEntity.GetRole.Id),
                    new SqlParameter("@locationId", updateEntity.GetLocation.Id),
                    new SqlParameter("@firstName", updateEntity.GetFirstName),
                    new SqlParameter("@lastName", updateEntity.GetLastName),
                    new SqlParameter("@email", updateEntity.GetEmail),
                    new SqlParameter("@password", updateEntity.GetPassword),
                    new SqlParameter("@salt", updateEntity.GetSalt),
                    new SqlParameter("@accessToken", updateEntity.GetAccessToken)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a user's password
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateUserPasswordAsync(IUser user, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spUpdateUserSecurity]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", user.GetUserId),
                    new SqlParameter("@userNewPassword", user.GetPassword),
                    new SqlParameter("@userNewSalt", user.GetSalt)

                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Extracts the user information from the data reader.
        /// </summary>
        /// <param name="reader">A reader containing the stream of rows.</param>
        /// <param name="cancellation">A token used to cancel the request.</param>
        /// <returns>A <see cref="IUser"/> with the extracted data from the reader.</returns>
        private static async Task<IUser> GetUserDataFromReaderAsync(SqlDataReader reader, CancellationToken cancellation)
        {
            string delimiterAreas = await GetColumnDataOrEmptyStringAsync(reader, 10, cancellation);
            IUser user = new User(
                id: reader.GetInt32(0),
                userRole: new Role(reader.GetInt32(1), reader.GetString(2), reader.GetString(3)),
                userLocation: new Location(reader.GetInt32(4), reader.GetString(5)),
                consultantAreas: SplitConsultantAreas(delimiterAreas),
                reader.GetString(6),
                reader.GetString(7),
                reader.GetString(8),
                accessToken: reader.GetString(9));
            return user;
        }

        /// <summary>
        /// Splits a delimitered string, containing the consultant areas.
        /// </summary>
        /// <param name="delimiterSeparatedValues">A string with the values to split.</param>
        /// <param name="separator">A separator used to determine the delimitered string.</param>
        /// <returns>A <see cref="List{Area}"/> containing the consultant areas; else empty list of none.</returns>
        private static List<Area> SplitConsultantAreas(string delimiterSeparatedValues, string separator = ";")
        {
            var splitConsultantAreas = delimiterSeparatedValues.Split(separator).ToList();
            List<Area> consultantAreas = new();

            foreach (var area in splitConsultantAreas)
            {
                consultantAreas.Add(new Area(0, area));
            }

            return consultantAreas;
        }

        /// <summary>
        /// Gets the data within the column.
        /// </summary>
        /// <remarks>
        /// Checks if the column in the reader is null from the DB.
        /// Is null, then returns empty string.
        /// Is not null, then returns the data within the column.
        /// </remarks>
        /// <param name="reader"></param>
        /// <param name="columnIndex"></param>
        /// <param name="cancellation"></param>
        /// <returns><see cref="string.Empty"/> if column data is null; otherwise the column data.</returns>
        private static async Task<string> GetColumnDataOrEmptyStringAsync(SqlDataReader reader, int columnIndex, CancellationToken cancellation)
        {
            return await Task.FromResult(reader.GetString(columnIndex)) ?? string.Empty;
        }        
    }
}
