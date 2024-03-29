﻿using BlazorServerWebsite.Data.Services.Abstractions;
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
    public class UserService : BaseService<IUserRepository, IUser>, IUserService
    {
        private readonly IAuthenticationAccess _access;

        public UserService(IUserRepository userRepository, IAuthenticationAccess access) : base(userRepository)
        {
            _access = access;
        }

        public override async Task<int> CreateAsync(IUser createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public override async Task<int> DeleteAsync(IUser deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public string GenerateAccessToken(IUser user)
        {
            return user is User u ? _access.GenerateAccessToken(u) : string.Empty;
        }

        public override async Task<IEnumerable<IUser>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public override async Task<IUser> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public ClaimsIdentity GetClaimsIdentity(IUser user)
        {
            return _access.GetClaimsIdentity((User)user);
        }

        public async Task<IUser> GetUserByAccessTokenAsync(string token, CancellationToken cancellation)
        {
            var user = new User(0, null, null, null, string.Empty, string.Empty, string.Empty, accessToken:token);
            return await Repository.GetUserByAccessTokenAsync(user.GetAccessToken, cancellation);
        }

        public async Task<IUser> GetUserByEmailAsync(string email, CancellationToken cancellation)
        {
            return await Repository.GetByEmailAsync(email, cancellation);
        }

        public async Task<int> GrantUserAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            return await Repository.GrantUserAreaAsync(user, areaId, cancellation);
        }

        public async Task<IUser> LoginAsync(string email, string password, CancellationToken cancellation)
        {
            // Get user's salt to hash the given password.
            var salt = await Repository.GetSaltByEmailAddressAsync(email, cancellation);

            if (string.IsNullOrEmpty(salt))
            {
                return null;
            }

            // Initialize the user.
            IUser _user = new User(
                0, null, null, null, "", "",
                email,
                password,
                salt: salt);

            // Hash Password.
            _user.HashPassword();

            // Get the task to load.
            var loginTask = Repository.AuthenticateUserLoginAsync(_user, cancellation);

            // Await the task to finish.
            await Task.WhenAll(loginTask);

            Console.WriteLine($"Login Result : {loginTask.Result}");

            if (loginTask.Result)
            {
                return await Repository.GetByEmailAsync(email, cancellation);
            }

            // Return null if the user isn't authorized.
            return null;
        }

        public async Task<int> RemoveUserAreaAsync(IUser user, int areaId, CancellationToken cancellation)
        {
            return await Repository.RemoveAreaAsync(user, areaId, cancellation);
        }

        public override async Task<int> UpdateAsync(IUser updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }

        public async Task<int> UpdateUserPasswordAsync(IUser user, CancellationToken cancellation)
        {
            return await Repository.UpdateUserPasswordAsync(user, cancellation);
        }

        public async Task<bool> ValidateUserExistsByEmail(string userEmail, CancellationToken cancellation)
        {
            var user = new User(0, null, null, null, string.Empty, string.Empty, userEmail);
            return await Repository.CheckUserExistsAsync(user, cancellation);
        }
    }
}
