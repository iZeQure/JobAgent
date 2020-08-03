using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

            // Execture command, catch return value.
            int returnBoolean = cmd.ExecuteNonQuery();

            // Check if the result is 0. - return false.
            if (returnBoolean == 0) return false;

            // Return true.
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

        public bool LogIn(string email, string password)
        {
            throw new NotImplementedException();
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
    }
}
