using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
using JobAgent.Data.Security;
using JobAgent.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CheckUserExists", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserEmail", email);

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
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("CreateUser", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("FirstName", create.FirstName);
            cmd.Parameters.AddWithValue("LastName", create.LastName);
            cmd.Parameters.AddWithValue("Email", create.Email);
            cmd.Parameters.AddWithValue("Password", create.Password);
            cmd.Parameters.AddWithValue("Salt", create.Salt);
            cmd.Parameters.AddWithValue("AccessToken", create.AccessToken);
            cmd.Parameters.AddWithValue("ConsultantAreaId", create.ConsultantArea.Id);
            cmd.Parameters.AddWithValue("LocationId", create.Location.Id);

            // Execute command, catch returned value.
            int returnValue = cmd.ExecuteNonQuery();
        }

        public IEnumerable<User> GetAll()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllUsers",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.SqlConnection
            };

            Database.Instance.OpenConnection();

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
                            LastName = r.GetString("LastName")
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
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByEmail", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserEmail", email);

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
                            Name = reader.GetString("ConsultantAreaName"),
                            Description = ""
                        },
                        Location = new Location()
                        {
                            Id = reader.GetInt32("LocationId"),
                            Name = reader.GetString("LocationName"), 
                            Description = ""
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
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetUserByAccessToken", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserAccessToken", accessToken);

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
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetUserSaltByEmail", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserEmail", email);

            // Initalize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    return reader.GetString(0);
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
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UserLogin", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserEmail", email);
            cmd.Parameters.AddWithValue("UserSecret", password);

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
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUser", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserId", update.Id).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("NewFirstName", update.FirstName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("NewLastName", update.LastName).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("NewEmail", update.Email).SqlDbType = SqlDbType.VarChar;
            cmd.Parameters.AddWithValue("NewConsultantAreaId", update.ConsultantArea.Id).SqlDbType = SqlDbType.Int;
            cmd.Parameters.AddWithValue("NewLocationId", update.Location.Id).SqlDbType = SqlDbType.Int;

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
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("ValidatePassword", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserPassword", password);

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
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateUserPassword", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("UserEmail", authorization.Email);
            cmd.Parameters.AddWithValue("UserSecret", authorization.Password);

            // Execute cmd.
            cmd.ExecuteNonQuery();
        }
    }
}
