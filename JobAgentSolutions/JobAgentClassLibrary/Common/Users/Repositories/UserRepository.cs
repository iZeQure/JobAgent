using Dapper;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Areas.Factory;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Users.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly UserEntityFactory _factory;
        private readonly AreaEntityFactory _areaFactory;

        public UserRepository(ISqlDbManager sqlDbManager, UserEntityFactory factory, AreaEntityFactory areaFactory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
            _areaFactory = areaFactory;
        }

        public async Task<bool> AuthenticateUserLoginAsync(IAuthUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), "User was null.");
            }

            bool isAuthenticated = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spValidateUserLogin]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@userEmail", ((AuthUser)user).Email);
                dynamicValues.Add("@userPassword", user.Password);
                dynamicValues.Add("@returnResult", SqlDbType.Bit, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                isAuthenticated = dynamicValues.Get<bool>("@returnResult");
            }

            return isAuthenticated;
        }

        public async Task<bool> CheckUserExistsAsync(IUser user)
        {
            bool userExists = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spValidateUserLogin]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@userEmail", user.Email);
                dynamicValues.Add("@returnResult", SqlDbType.Bit, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                userExists = dynamicValues.Get<bool>("@returnResult");
            }

            return userExists;
        }

        public async Task<IUser> CreateAsync(IUser entity)
        {
            int entityId = 0;

            if (entity is AuthUser authUser)
            {
                using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
                {
                    var proc = "[JA.spCreateUser]";
                    var values = new
                    {
                        @roleId = authUser.RoleId,
                        @locationId = authUser.LocationId,
                        @userFirstName = authUser.FirstName,
                        @userLastName = authUser.LastName,
                        @userEmail = authUser.Email,
                        @userPass = authUser.Password,
                        @userSalt = authUser.Salt,
                        @userAccessToken = authUser.AccessToken
                    };

                    entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
                }

                if (entityId != 0)
                {
                    return await GetByIdAsync(entityId);
                }
            }

            return null;
        }

        public async Task<List<IUser>> GetAllAsync()
        {
            List<IUser> users = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetUsers]";

                var queryResults = await conn.QueryAsync<UserInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResults is not null)
                {
                    foreach (var result in queryResults)
                    {
                        IUser user = (IUser)_factory.CreateEntity(nameof(User),
                            result.Id,
                            result.RoleId,
                            result.LocationId,
                            result.FirstName,
                            result.LastName,
                            result.Email);

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public async Task<IUser> GetByEmailAsync(string email)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetUserByEmail]";
                var values = new
                {
                    @email = email
                };

                var result = await conn.QueryFirstOrDefaultAsync<UserInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (result is not null)
                {
                    user = (IUser)_factory.CreateEntity(nameof(User),
                        result.Id,
                        result.RoleId,
                        result.LocationId,
                        result.FirstName,
                        result.LastName,
                        result.Email);
                }
            }

            return user;
        }

        public async Task<IUser> GetByIdAsync(int id)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetUserById]";
                var values = new
                {
                    @userId = id
                };

                var result = await conn.QueryFirstOrDefaultAsync<UserInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (result is not null)
                {
                    user = (IUser)_factory.CreateEntity(nameof(User),
                        result.Id,
                        result.RoleId,
                        result.LocationId,
                        result.FirstName,
                        result.LastName,
                        result.Email);
                }
            }

            return user;
        }

        public async Task<string> GetSaltByEmailAsync(string email)
        {
            string salt = string.Empty;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetUserSaltByEmail]";
                var values = new
                {
                    @userEmail = email
                };

                var result = await conn.QueryFirstOrDefaultAsync<string>(proc, values, commandType: CommandType.StoredProcedure);

                if (result is not null)
                {
                    salt = result;
                }
            }

            return salt;
        }

        public async Task<IUser> GetUserByAccessTokenAsync(string accessToken)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetUserByAccessToken]";
                var values = new
                {
                    @userAccessToken = accessToken
                };

                var result = await conn.QueryFirstOrDefaultAsync<AuthUserView>(proc, values, commandType: CommandType.StoredProcedure);

                if (result is not null)
                {
                    user = (AuthUser)_factory.CreateEntity(nameof(AuthUser),
                        result.Id,
                        result.RoleId,
                        result.LocationId,
                        result.FirstName,
                        result.LastName,
                        result.Email,
                        result.AccessToken);
                }
            }

            return user;
        }

        public async Task<int> GrantAreaToUserAsync(IUser user, int areaId)
        {
            int affectedRows = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@userId", user.Id),
                    new SqlParameter("@areaId", areaId)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGrantUserArea]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        affectedRows = await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return affectedRows;
        }

        public async Task<int> RevokeAreaFromUserAsync(IUser user, int areaId)
        {
            int affectedRows = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@userId", user.Id),
                    new SqlParameter("@areaId", areaId)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spRemoveUserArea]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        affectedRows = await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return affectedRows;
        }

        public async Task<bool> RemoveAsync(IUser entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@userId", entity.Id)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spRemoveUser]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        isDeleted = (await cmd.ExecuteNonQueryAsync()) == 1;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return isDeleted;
        }

        public async Task<IUser> UpdateAsync(IUser entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@userId", entity.Id),
                    new SqlParameter("@roleId", entity.RoleId.Id),
                    new SqlParameter("@locationId", entity.LocationId.Id),
                    new SqlParameter("@firstName", entity.FirstName),
                    new SqlParameter("@lastName", entity.LastName),
                    new SqlParameter("@email", entity.Email)
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spUpdateUser]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        entityId = (int)await cmd.ExecuteScalarAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<int> UpdateUserPasswordAsync(IAuthUser user)
        {
            int affectedRows = 0;

            if (user is AuthUser authUser)
            {
                using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
                {
                    var values = new SqlParameter[]
                    {
                    new SqlParameter("@userId", authUser.Id),
                    new SqlParameter("@userNewPassword", authUser.Password),
                    new SqlParameter("@userNewSalt", authUser.Salt)
                    };

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "[JA.spUpdateUser]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(values);

                        try
                        {
                            await conn.OpenAsync();

                            affectedRows = await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }

            return affectedRows;
        }

        public async Task<IUser> GetUserConsultantAreasAsync(IUser user)
        {
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetUserConsultantAreasByUserId]";

                var queryResult = await conn.QueryAsync<AreaInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IArea area = (IArea)_areaFactory.CreateEntity(
                                nameof(Area),
                                result.Id, result.Name);

                        user.ConsultantAreas.Add(area);
                    }
                }
            }
            return user;
        }
    }
}
}
