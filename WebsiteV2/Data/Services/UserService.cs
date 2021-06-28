using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using SecurityLibrary.Cryptography.Extentions;
using SecurityLibrary.Interfaces;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class UserService : BaseService<IUserRepository>, IUserService
    {
        private readonly IAccess _access;

        public UserService(IUserRepository userRepository, IAccess access) : base(userRepository)
        {
            _access = access;
        }

        public Task<bool> CheckUserExistsAsync(IUser user, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public string GenerateAccessToken(IUser user)
        {
            return user is User u ? _access.GenerateAccessToken(u) : string.Empty;
        }

        public ClaimsIdentity GetClaimsIdentity(IUser user)
        {
            return _access.GetClaimsIdentity((User)user);
        }

        public async Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            return await Repository.GrantUserAreaAsync(user, areaId, cancellation);
        }

        public Task<IUser> LoginAsync(IUser user, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> RegisterUserAsync(IUser user, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RemoveUserAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            return await Repository.RemoveAreaAsync(user, areaId, cancellation);
        }
    }
}
