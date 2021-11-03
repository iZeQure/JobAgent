using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDbManager _sqlDbManager;

        public UserRepository(ISqlDbManager sqlDbManager)
        {
            _sqlDbManager = sqlDbManager;
        }

        public async Task<bool> AuthenticateUserLoginAsync(IAuthUser user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), "User was null.");
            }

            bool isAuthenticated = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Complex))
            {
                var outputParam = new SqlParameter
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@returnResult",
                    SqlDbType = SqlDbType.Bit
                };

                var values = new SqlParameter[]
                {
                    new SqlParameter("@userEmail", ((AuthUser)user).Email),
                    new SqlParameter("@userPassword", user.Password),
                    outputParam
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spValidateUserLogin]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                isAuthenticated = Convert.ToBoolean(values[2].Value);
            }

            return isAuthenticated;
        }

        public async Task<bool> CheckUserExistsAsync(IUser user)
        {
            bool userExists = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Complex))
            {
                var outputParam = new SqlParameter
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "@returnResult",
                    SqlDbType = SqlDbType.Bit
                };

                var values = new SqlParameter[]
                {
                    new SqlParameter("@userEmail", user.Email),
                    outputParam
                };

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spValidateUserExists]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(values);

                    try
                    {
                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                userExists = Convert.ToBoolean(values[2].Value);
            }

            return userExists;
        }

        public async Task<IUser> CreateAsync(IUser entity)
        {
            int entityId = 0;

            if (entity is AuthUser authUser)
            {
                using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
                {
                    var values = new SqlParameter[]
                    {
                        new SqlParameter("@roleId", authUser.Role.Id),
                        new SqlParameter("@locationId", authUser.Location.Id),
                        new SqlParameter("@userFirstName", authUser.FirstName),
                        new SqlParameter("@userLastName", authUser.LastName),
                        new SqlParameter("@userEmail", authUser.Email),
                        new SqlParameter("@userPass", authUser.Password),
                        new SqlParameter("@userSalt", authUser.Salt),
                        new SqlParameter("@userAccessToken", authUser.AccessToken)
                    };

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "[JA.spCreateUser]";
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
            }

            return null;
        }

        public async Task<List<IUser>> GetAllAsync()
        {
            List<IUser> users = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetUsers]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return users;

                            while (await reader.ReadAsync())
                            {
                                var user = await MapUserDataFromSqlDataReader(reader);

                                users.Add(user);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return users;
        }

        public async Task<IUser> GetByEmailAsync(string email)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetUserByEmail]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@email", email);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                user = await MapUserDataFromSqlDataReader(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return user;
        }

        public async Task<IUser> GetByIdAsync(int id)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetUserById]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userId", id);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                user = await MapUserDataFromSqlDataReader(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return user;
        }

        public async Task<string> GetSaltByEmailAddressAsync(string email)
        {
            string salt = string.Empty;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetUserSaltByEmail]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userEmail ", email);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                salt = reader.GetString(0);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return salt;
        }

        public async Task<IUser> GetUserByAccessTokenAsync(string accessToken)
        {
            IUser user = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "[JA.spGetUserByAccessToken]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@userAccessToken ", accessToken);

                    try
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (!reader.HasRows) return null;

                            while (await reader.ReadAsync())
                            {
                                user = await MapUserDataFromSqlDataReader(reader);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return user;
        }

        public async Task<int> GrantAreaToUserAsync(IUser user, int areaId)
        {
            int affectedRows = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
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

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var values = new SqlParameter[]
                {
                    new SqlParameter("@userId", entity.Id),
                    new SqlParameter("@roleId", entity.Role.Id),
                    new SqlParameter("@locationId", entity.Location.Id),
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
                using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
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

        private async Task<IUser> MapUserDataFromSqlDataReader(SqlDataReader reader)
        {
            bool isConsultantAreasNull = await reader.IsDBNullAsync("Consultant Areas");
            var delimiteredConsultantAreas = isConsultantAreasNull == true ? string.Empty : reader.GetString(10);
            var splitConsultantAreas = delimiteredConsultantAreas.Split(";");
            var consultantAreas = new List<Area>();

            foreach (var area in splitConsultantAreas)
            {
                consultantAreas.Add(new Area
                {
                    Id = 0,
                    Name = area
                });
            }

            IUser user = new AuthUser
            {
                Id = reader.GetInt32(0),
                Role = new Role
                {
                    Id = reader.GetInt32(1),
                    Name = reader.GetString(2),
                    Description = reader.GetString(3)
                },
                Location = new Location
                {
                    Id = reader.GetInt32(4),
                    Name = reader.GetString(5)
                },
                FirstName = reader.GetString(6),
                LastName = reader.GetString(7),
                Email = reader.GetString(8),
                AccessToken = reader.GetString(9),
                ConsultantAreas = consultantAreas
            };

            return user;
        }
    }
}
