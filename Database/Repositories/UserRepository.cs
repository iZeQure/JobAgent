using DataAccess.Repositories.Interfaces;
using DataAccess.SqlAccess;
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
        /// <summary>
        /// Check if the user email exists.
        /// </summary>
        /// <param name="email">Used to define the user lookup.</param>
        /// <returns>True or False</returns>
        public async Task<bool> CheckUserExists(string email)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CheckUserExists", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Define return parameters.
            var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            // Execute command, catch return value.
            await cmd.ExecuteNonQueryAsync();

            var result = returnParameter.Value;

            // Check if return value is false.
            if ((int)result == 0) return false;

            return true;
        }

        public async void Create(User create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CreateUser", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

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
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllUsers",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            await db.OpenConnectionAsync();

            // Initialzie data reader.
            using SqlDataReader r = await c.ExecuteReaderAsync();

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

        public async Task<User> GetUserByEmail(string email)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary user obj.
            User tempUser = new User();

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByEmail", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Initialzie data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

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

        public async Task<User> GetUserByAccessToken(string accessToken)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary user obj.
            User tempUser = new User();

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByAccessToken", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@token", accessToken);

            // Initialzie data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
            using var db = SqlDatabaseAccess.SqlInstance;

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetUserSaltByEmail", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Initalize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

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

        public async Task<User> LogIn(string email, string password)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary obj.
            User tempUser = new User();

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UserLogin", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@secret", password);

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

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

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public async void Update(User update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUser", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Identifier).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@firstName", update.FirstName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@lastName", update.LastName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@email", update.Email).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@consultantAreaId", update.ConsultantArea.Identifier).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@locationId", update.Location.Identifier).SqlDbType = SqlDbType.Int;

            // Exectute command, catch return value.
            int returnValue = await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Validate user password.
        /// </summary>
        /// <param name="password">Used to specify the secret for the user.</param>
        /// <returns>True or false</returns>
        public async Task<bool> ValidatePassword(string password)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Open connection to datbase.
            await db.OpenConnectionAsync();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("ValidatePassword", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@secret", password);

            // Define return parameters.
            var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            // Execute command, catch return value.
            await cmd.ExecuteNonQueryAsync();

            var result = returnParameter.Value;

            // Check if return value is false.
            if ((int)result == 0) return false;

            return true;
        }

        public async void UpdateUserPassword(User authorization)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUserPassword", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", authorization.Email);
            cmd.Parameters.AddWithValue("@secret", authorization.Password);

            // Execute cmd.
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
