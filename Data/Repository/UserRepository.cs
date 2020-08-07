using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
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
        private readonly JWTSettings jwtSettings;
        private JwtSecurityTokenHandler JwtSecurityTokenHandler { get; set; }

        public UserRepository() { }

        public UserRepository(IOptions<JWTSettings> jwt)
        {
            jwtSettings = jwt.Value;
        }

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
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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

        public RefreshTokenModel GenerateRefreshToken()
        {
            RefreshTokenModel refreshToken = new RefreshTokenModel();

            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken.Token = Convert.ToBase64String(randomNumber);
            }
            refreshToken.ExpiryDate = DateTime.UtcNow.AddMonths(6);

            return refreshToken;
        }

        public string GenerateAccessToken(int id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(id))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
