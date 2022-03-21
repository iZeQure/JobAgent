using Dapper;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Areas.Factory;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Users.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ISqlDbManager _sqlDbManager;
        private readonly UserEntityFactory _factory;
        private readonly AreaEntityFactory _areaFactory;

        public UserRepository(ISqlDbManager sqlDbManager, UserEntityFactory factory, AreaEntityFactory areaFactory, ICryptographyService cryptographyService)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
            _areaFactory = areaFactory;
            _cryptographyService = cryptographyService;
        }

        public async Task<bool> AuthenticateUserLoginAsync(IAuthUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), "User was null.");
            }

            var userSalt = await GetSaltByEmailAsync(((AuthUser)user).Email);
            if (string.IsNullOrEmpty(userSalt))
            {
                return false;
            }

            bool isAuthenticated = false;
            var hashedUserPassword = _cryptographyService.HashUserPasswordWithSalt(((AuthUser)user).Password, userSalt);

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spValidateUserLogin]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@userEmail", ((AuthUser)user).Email);
                dynamicValues.Add("@userPassword", hashedUserPassword);
                dynamicValues.Add("@returnResult", SqlDbType.Bit, direction: ParameterDirection.Output);

                isAuthenticated = await conn.QueryFirstOrDefaultAsync<bool>(proc, dynamicValues, commandType: CommandType.StoredProcedure);
            }

            return isAuthenticated;
        }

        public async Task<bool> CheckUserExistsAsync(string email)
        {
            bool userExists = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spValidateUserExists]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@userEmail", email);
                dynamicValues.Add("@returnResult", SqlDbType.Bit, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                var returnResult = dynamicValues.Get<int>("@returnResult");

                if (returnResult == 1)
                {
                    userExists = true;
                }
            }

            return userExists;
        }

        public async Task<IUser> CreateAsync(IUser entity)
        {
            int entityId = 0;

            if (entity is AuthUser authUser)
            {
                IHashedUser hashedUser = _cryptographyService.CreateHashedUser(authUser.Password);

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
                        @userPass = hashedUser.Password,
                        @userSalt = hashedUser.Salt,
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
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

        public async Task<IAuthUser> GetUserByAccessTokenAsync(string accessToken)
        {
            IAuthUser authUser = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spGetUserByAccessToken]";
                var values = new
                {
                    @userAccessToken = accessToken
                };

                var result = await conn.QueryFirstOrDefaultAsync<AuthUserView>(proc, values, commandType: CommandType.StoredProcedure);

                if (result is not null)
                {
                    authUser = (AuthUser)_factory.CreateEntity(nameof(AuthUser),
                        result.UserId,
                        result.RoleId,
                        result.LocationId,
                        result.FirstName,
                        result.LastName,
                        result.Email,
                        "");
                }
            }

            return authUser;
        }

        public async Task<bool> GrantUserConsultantAreaAsync(IUser user, int areaId)
        {
            bool isGranted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                var proc = "[JA.spGrantUserArea]";
                var values = new
                {
                    @userId = user.Id,
                    @areaId = areaId
                };

                isGranted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isGranted;
        }

        public async Task<bool> RevokeUserConsultantAreaAsync(IUser user, int areaId)
        {
            bool isRevoked = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var proc = "[JA.spRemoveUserArea]";
                var values = new
                {
                    @userId = user.Id,
                    @areaId = areaId
                };

                isRevoked = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isRevoked;
        }

        public async Task<bool> RemoveAsync(IUser entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveUser]";
                var values = new
                {
                    @userId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IUser> UpdateAsync(IUser entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateUser]";
                var values = new
                {
                    @userId = entity.Id,
                    @roleID = entity.RoleId,
                    @locationId = entity.LocationId,
                    @userFirstName = entity.FirstName,
                    @userLastName = entity.LastName,
                    @userEmail = entity.Email
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<bool> UpdateUserPasswordAsync(IAuthUser user)
        {
            bool passIsUpdated = false;

            if (user is AuthUser authUser)
            {
                IHashedUser hashedUser = _cryptographyService.CreateHashedUser(authUser.Password);

                using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
                {
                    var proc = "[JA.spUpdateUserSecurity]";
                    var values = new
                    {
                        @userId = authUser.Id,
                        @userNewPassword = hashedUser.Password,
                        @userNewSalt = hashedUser.Salt
                    };

                    passIsUpdated = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
                }
            }

            return passIsUpdated;
        }

        public async Task<bool> UpdateUserAccessTokenAsync(IAuthUser user)
        {
            bool updatedToken = false;

            if (user is AuthUser authUser)
            {
                using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
                {
                    var proc = "[JA.spUpdateUserAccessToken]";
                    var values = new
                    {
                        @userEmail = authUser.Email,
                        @userAccessToken = authUser.AccessToken,
                    };

                    updatedToken = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
                }
            }

            return updatedToken;
        }

        public async Task<List<IArea>> GetUserConsultantAreasAsync(IUser user)
        {
            List<IArea> areas = new();
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
            {
                string proc = "[JA.spGetUserConsultantAreasByUserId]";
                var values = new
                {
                    @userId = user.Id
                };

                var queryResult = await conn.QueryAsync<AreaInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IArea area = (IArea)_areaFactory.CreateEntity(
                                nameof(Area),
                                result.Id, result.Name);

                        areas.Add(area);
                    }
                }
            }
            return areas;
        }

        public async Task<bool> ValidateUserAccessTokenAsync(string accessToken)
        {
            bool validatedToken = false;

            if (accessToken != null && accessToken != string.Empty)
            {
                using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.ComplexUser))
                {
                    var proc = "[JA.spValidateUserAccessToken]";
                    var dynamicValues = new DynamicParameters();

                    dynamicValues.Add("@userAccessToken", accessToken);
                    dynamicValues.Add("@returnResult", SqlDbType.Bit, direction: ParameterDirection.Output);

                    await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                    var returnResult = dynamicValues.Get<int>("@returnResult");

                    if (returnResult == 1)
                    {
                        validatedToken = true;
                    }
                }
            }

            return validatedToken;
        }
    }
}