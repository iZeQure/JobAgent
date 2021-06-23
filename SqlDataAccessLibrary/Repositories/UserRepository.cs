using ObjectLibrary.Common;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
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
        public async Task<bool> AuthenticateUserLoginAsync(User user, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spValidateUserLogin]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@returnResult", SqlDbType = SqlDbType.Bit };

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userEmail", user.GetEmail),
                    new SqlParameter("@userPassword", user.Password),
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
        /// Checks if user exists in database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<bool> CheckUserExistsAsync(User user, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spValidateUserExists]";

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
        /// Creates new User
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(User createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateUser];";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@RoleId", createEntity.GetRole.Id),
                    new SqlParameter("@LocationId", createEntity.GetLocation.Id),
                    new SqlParameter("@FirstName", createEntity.GetFirstName),
                    new SqlParameter("@LastName", createEntity.GetLastName),
                    new SqlParameter("@Email", createEntity.GetEmail),
                    new SqlParameter("@Password", createEntity.Password),
                    new SqlParameter("@Salt", createEntity.Salt),
                    new SqlParameter("@AccessToken", createEntity.AccessToken)
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
        public async Task<int> DeleteAsync(User deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveUser]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", deleteEntity.Id)
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
        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetUsers]";
                List<User> tempUserList = new();

                SqlParameter[] parameters = new SqlParameter[] { };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        User tempUser = new(
                            id: reader.GetInt32(0),
                            firstName: reader.GetString(1),
                            lastName: reader.GetString(2),
                            email: reader.GetString(4),
                            userRole: new Role(0, reader.GetString(5), ""),
                            userLocation: new Location(0, reader.GetString(6)),
                            consultantAreas: null
                            );

                        tempUserList.Add(tempUser);
                    }
                }

                return tempUserList;
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
        public async Task<User> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetUserById]";


                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userAccessToken", id),
                };


                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        User tempuser = new(


                            id: reader.GetInt32(0),
                            firstName: reader.GetString(1),
                            lastName: reader.GetString(2),
                            email: reader.GetString(4),
                            userRole: new Role(0, reader.GetString(5), ""),
                            userLocation: new Location(0, reader.GetString(6)),
                            consultantAreas: null

                            );

                        return tempuser;
                    }
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
        public async Task<User> GetUserByAccessTokenAsync(string accessToken, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetUserByAccessToken]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userAccessToken", accessToken),
                };


                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        User tempuser = new(


                            id: reader.GetInt32(0),
                            firstName: reader.GetString(1),
                            lastName: reader.GetString(2),
                            email: reader.GetString(4),
                            userRole: new Role(0, reader.GetString(5), ""),
                            userLocation: new Location(0, reader.GetString(6)),
                            consultantAreas: null

                            );

                        return tempuser;
                    }
                }

                return null;
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
        public async Task<int> UpdateAsync(User updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateUser]";

                SqlParameter[] parameters = new[]
                {
                new SqlParameter("@userId", updateEntity.Id),
                new SqlParameter("@RoleId", updateEntity.GetRole.Id),
                new SqlParameter("@LocationId", updateEntity.GetLocation.Id),
                new SqlParameter("@FirstName", updateEntity.GetFirstName),
                new SqlParameter("@LastName", updateEntity.GetLastName),
                new SqlParameter("@Email", updateEntity.GetEmail),
                new SqlParameter("@Password", updateEntity.Password),
                new SqlParameter("@Salt", updateEntity.Salt),
                new SqlParameter("@AccessToken", updateEntity.AccessToken)
            };

                //Needs to also update ConsultingAreas

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
        public async Task<int> UpdateUserPasswordAsync(User user, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateUserSecurity]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@userId", user.Id),
                    new SqlParameter("userNewPassword", user.Id),
                    new SqlParameter("@userldPassword", user.Password),
                    new SqlParameter("@userNewSalt", user.Salt),
                    new SqlParameter("@resultReturn", user.Id)

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
