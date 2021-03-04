using DataAccess.Repositories.Interfaces;
using DataAccess.SqlAccess;
using DataAccess.SqlAccess.Interfaces;
using Pocos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public UserRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Check if the user email exists.
        /// </summary>
        /// <param name="email">Used to define the user lookup.</param>
        /// <returns>True or False</returns>
        public async Task<bool> CheckUserExists(string email)
        {
            try
            {
                // Initialzie command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("CheckUserExists", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@email", email);

                // Define return parameters.
                var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command, catch return value.
                await cmd.ExecuteNonQueryAsync();

                var result = returnParameter.Value;

                // Check if return value is false.
                if ((int)result == 0) return false;

                return true;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task Create(User create)
        {
            try
            {
                // Initialzie command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("CreateUser", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@firstName", create.FirstName);
                cmd.Parameters.AddWithValue("@lastName", create.LastName);
                cmd.Parameters.AddWithValue("@email", create.Email);
                cmd.Parameters.AddWithValue("@password", create.Password);
                cmd.Parameters.AddWithValue("@salt", create.Salt);
                cmd.Parameters.AddWithValue("@accessToken", create.AccessToken);
                cmd.Parameters.AddWithValue("@consultantAreaId", create.ConsultantArea.Identifier);
                cmd.Parameters.AddWithValue("@locationId", create.Location.Identifier);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("GetAllUsers", CommandType.StoredProcedure);

                // Initialzie data reader.
                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                // Temporary list.
                List<User> tempUsers = new List<User>();

                // Check for any data.
                if (r.HasRows)
                {
                    // Read data.
                    while (await r.ReadAsync())
                    {
                        tempUsers.Add(
                            new User
                            {
                                Identifier = r.GetInt32("Id"),
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName"),
                                Email = r.GetString("Email"),
                                ConsultantArea = new ConsultantArea()
                                {
                                    Name = r.GetString("ConsultantAreaName")
                                },
                                Location = new Location()
                                {
                                    Name = r.GetString("LocationName"),
                                    Description = !DataReaderExtensions.IsDBNull(r, "LocationDesc") ? r.GetString("LocationDesc") : string.Empty
                                }
                            });
                    }
                }

                // Return data list.
                return tempUsers;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                // Initialize temporary user obj.
                User tempUser = new User();

                // Initialzie command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetUserByEmail", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@email", email);

                // Initialzie data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check for data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (await reader.ReadAsync())
                    {
                        tempUser = new User()
                        {
                            Identifier = reader.GetInt32("UserId"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email"),
                            ConsultantArea = new ConsultantArea()
                            {
                                Identifier = reader.GetInt32("ConsultantAreaId"),
                                Name = reader.GetString("ConsultantAreaName")
                            },
                            Location = new Location()
                            {
                                Identifier = reader.GetInt32("LocationId"),
                                Name = reader.GetString("LocationName")
                            }
                        };
                    }
                }

                return tempUser;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            try
            {
                // Initialize temporary user obj.
                User tempUser = new User();

                // Initialzie command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetUserByAccessToken", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@token", accessToken);

                // Initialzie data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check for data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (await reader.ReadAsync())
                    {
                        tempUser = new User()
                        {
                            Identifier = reader.GetInt32("UserId"),
                            FirstName = reader.GetString("FirstName"),
                            LastName = reader.GetString("LastName"),
                            Email = reader.GetString("Email"),
                            AccessToken = reader.GetString("AccessToken"),
                            ConsultantArea = new ConsultantArea()
                            {
                                Identifier = reader.GetInt32("ConsultantAreaId"),
                                Name = reader.GetString("ConsultantAreaName")
                            },
                            Location = new Location()
                            {
                                Identifier = reader.GetInt32("LocationId"),
                                Name = reader.GetString("LocationName")
                            }
                        };
                    }
                }

                return tempUser;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public Task<User> GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user salt by email.
        /// </summary>
        /// <param name="email">Used to specify the user.</param>
        /// <returns>A salt for the user.</returns>
        public async Task<string> GetUserSaltByEmail(string email)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetUserSaltByEmail", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@email", email);

                // Initalize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check if any data.
                if (reader.HasRows)
                {
                    // Read the data.
                    while (await reader.ReadAsync())
                    {
                        return reader.GetString("Salt");
                    }
                }

                // Return empty if nothing.
                return string.Empty;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<User> LogIn(string email, string password)
        {
            try
            {
                // Initialize temporary obj.
                User tempUser = new User();

                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("UserLogin", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@secret", password);

                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check for data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (await reader.ReadAsync())
                    {
                        tempUser.Identifier = reader.GetInt32("UserId");
                        tempUser.FirstName = reader.GetString("FirstName");
                        tempUser.LastName = reader.GetString("LastName");
                        tempUser.Email = reader.GetString("Email");
                        tempUser.AccessToken = reader.GetString("AccessToken");
                        tempUser.ConsultantArea = new ConsultantArea()
                        {
                            Identifier = reader.GetInt32("ConsultantAreaId"),
                            Name = reader.GetString("ConsultantAreaName")
                        };
                        tempUser.Location = new Location()
                        {
                            Identifier = reader.GetInt32("LocationId"),
                            Name = reader.GetString("LocationName")
                        };
                    }
                }

                return tempUser;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public Task Remove(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Update(User update)
        {
            try
            {
                // Initialzie command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("UpdateUser", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@firstName", update.FirstName).SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@lastName", update.LastName).SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@email", update.Email).SqlDbType = SqlDbType.VarChar;
                cmd.Parameters.AddWithValue("@consultantAreaId", update.ConsultantArea.Identifier).SqlDbType = SqlDbType.Int;
                cmd.Parameters.AddWithValue("@locationId", update.Location.Identifier).SqlDbType = SqlDbType.Int;

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Exectute command
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Validate user password.
        /// </summary>
        /// <param name="password">Used to specify the secret for the user.</param>
        /// <returns>True or false</returns>
        public async Task<bool> ValidatePassword(string password)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("ValidatePassword", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@secret", password);

                // Define return parameters.
                var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                // Open connection to datbase.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command, catch return value.
                await cmd.ExecuteNonQueryAsync();

                var result = returnParameter.Value;

                // Check if return value is false.
                if ((int)result == 0) return false;

                return true;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task UpdateUserPassword(User authorization)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("UpdateUserPassword", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@email", authorization.Email);
                cmd.Parameters.AddWithValue("@secret", authorization.Password);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute cmd.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
