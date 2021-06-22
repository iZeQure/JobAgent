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

        public Task<IUser> AuthenticateUserLoginAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckUserExistsAsync(User user)
        {
            throw new NotImplementedException();
        }

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

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByAccessTokenAsync(string accessToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(User updateEntity, CancellationToken cancellation)
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

            return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);


        }

        public Task<int> UpdateUserPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
