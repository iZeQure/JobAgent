using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace JobAgent.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Check if the user email exists.
        /// </summary>
        /// <param name="email">Used to define the user lookup.</param>
        /// <returns>True or False</returns>
        public bool CheckUserExists(string email)
        {
            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CheckUserExists", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Define return parameters.
            var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            // Execute command, catch return value.
            cmd.ExecuteNonQuery();

            var result = returnParameter.Value;

            // Check if return value is false.
            if ((int)result == 0) return false;

            return true;
        }

        public void Create(User create)
        {
            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CreateUser", SqlDataAccess.Instance.SqlConnection)
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
            cmd.Parameters.AddWithValue("@consultantAreaId", create.ConsultantArea.Id);
            cmd.Parameters.AddWithValue("@locationId", create.Location.Id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<User> GetAll()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllUsers",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            SqlDataAccess.Instance.OpenConnection();

            // Initialzie data reader.
            using SqlDataReader r = c.ExecuteReader();

            // Temporary list.
            List<User> tempUsers = new List<User>();

            // Check for any data.
            if (r.HasRows)
            {
                // Read data.
                while (r.Read())
                {
                    tempUsers.Add(
                        new User
                        {
                            Id = r.GetInt32("Id"),
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

        public User GetUserByEmail(string email)
        {
            // Initialize temporary user obj.
            User tempUser = new User();

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByEmail", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Initialzie data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempUser = new User()
                    {
                        Id = reader.GetInt32("UserId"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        ConsultantArea = new ConsultantArea()
                        {
                            Id = reader.GetInt32("ConsultantAreaId"),
                            Name = reader.GetString("ConsultantAreaName")
                        },
                        Location = new Location()
                        {
                            Id = reader.GetInt32("LocationId"),
                            Name = reader.GetString("LocationName")
                        }
                    };
                }
            }
            return tempUser;
        }

        public User GetUserByAccessToken(string accessToken)
        {
            // Initialize temporary user obj.
            User tempUser = new User();

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByAccessToken", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@token", accessToken);

            // Initialzie data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempUser = new User()
                    {
                        Id = reader.GetInt32("UserId"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Email = reader.GetString("Email"),
                        AccessToken = reader.GetString("AccessToken"),
                        ConsultantArea = new ConsultantArea()
                        {
                            Id = reader.GetInt32("ConsultantAreaId"),
                            Name = reader.GetString("ConsultantAreaName")
                        },
                        Location = new Location()
                        {
                            Id = reader.GetInt32("LocationId"),
                            Name = reader.GetString("LocationName")
                        }
                    };
                }
            }
            return tempUser;
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user salt by email.
        /// </summary>
        /// <param name="email">Used to specify the user.</param>
        /// <returns>A salt for the user.</returns>
        public string GetUserSaltByEmail(string email)
        {
            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetUserSaltByEmail", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);

            // Initalize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    return reader.GetString("Salt");
                }
            }

            // Return empty if nothing.
            return string.Empty;
        }

        public User LogIn(string email, string password)
        {
            // Initialize temporary obj.
            User tempUser = new User();

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UserLogin", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@secret", password);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempUser.Id = reader.GetInt32("UserId");
                    tempUser.FirstName = reader.GetString("FirstName");
                    tempUser.LastName = reader.GetString("LastName");
                    tempUser.Email = reader.GetString("Email");
                    tempUser.AccessToken = reader.GetString("AccessToken");
                    tempUser.ConsultantArea = new ConsultantArea()
                    {
                        Id = reader.GetInt32("ConsultantAreaId"),
                        Name = reader.GetString("ConsultantAreaName")
                    };
                    tempUser.Location = new Location()
                    {
                        Id = reader.GetInt32("LocationId"),
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

        public void Update(User update)
        {
            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUser", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Id).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@firstName", update.FirstName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@lastName", update.LastName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@email", update.Email).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("@consultantAreaId", update.ConsultantArea.Id).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("@locationId", update.Location.Id).SqlDbType = SqlDbType.Int;

            // Exectute command, catch return value.
            int returnValue = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Validate user password.
        /// </summary>
        /// <param name="password">Used to specify the secret for the user.</param>
        /// <returns>True or false</returns>
        public bool ValidatePassword(string password)
        {
            // Open connection to datbase.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("ValidatePassword", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@secret", password);

            // Define return parameters.
            var returnParameter = cmd.Parameters.AddWithValue("return_value", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            // Execute command, catch return value.
            cmd.ExecuteNonQuery();

            var result = returnParameter.Value;

            // Check if return value is false.
            if ((int)result == 0) return false;

            return true;
        }

        public void UpdateUserPassword(User authorization)
        {
            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUserPassword", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@email", authorization.Email);
            cmd.Parameters.AddWithValue("@secret", authorization.Password);

            // Execute cmd.
            cmd.ExecuteNonQuery();
        }
    }
}
