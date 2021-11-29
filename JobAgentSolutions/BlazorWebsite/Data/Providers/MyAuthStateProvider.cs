using Blazored.LocalStorage;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Security.interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Data.Providers
{
    public class MyAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationAccess _access;
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// Access the value for the access token in the memory.
        /// </summary>
        private const string ACCESS_TOKEN = "AccessToken";

        public MyAuthStateProvider(IUserService userService, IAuthenticationAccess userAccess, ILocalStorageService localStorageService)
        {
            _userService = userService;
            _access = userAccess;
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Declare a variable to store the identity.
            ClaimsIdentity identity;
            ClaimsPrincipal principal;

            // Declare a varible to store the user.
            IAuthUser user;

            // Get the access token, from current auth state.
            string accessToken = await _localStorageService.GetItemAsync<string>(ACCESS_TOKEN);

            if (!string.IsNullOrWhiteSpace(accessToken) || !string.IsNullOrEmpty(accessToken))
            {
                // Validate Token.
                var accessTokenIsValid = await _userService.ValidateUserAccessTokenAsync(accessToken);

                if (accessTokenIsValid)
                {
                    // Get the user by access token.
                    user = await _userService.GetUserByAccessTokenAsync(accessToken);

                    // TODO : Call get claims identity, to store the user into.
                    // Get claims identity with the authenticated user.
                    identity = await _access.GetClaimsIdentityAsync(user);

                    principal = new ClaimsPrincipal(identity);
                    return new AuthenticationState(principal);
                }
            }

            identity = new ClaimsIdentity();
            principal = new ClaimsPrincipal(identity);

            return new AuthenticationState(principal);
        }

        /// <summary>
        /// Mark user as authenticated, if credentials is valid from server side.
        /// </summary>
        /// <param name="genericAuthUser">Used to authenticate the current user.</param>
        /// <returns>A authentication notification task changed.</returns>
        public async Task MarkUserAsAuthenticated(IAuthUser genericAuthUser)
        {
            if (string.IsNullOrEmpty(genericAuthUser.AccessToken))
            {
                throw new NullReferenceException("Couldn't authenticate user. Access token not found.");
            }

            // Set the access token in the local memory.
            await _localStorageService.SetItemAsync(ACCESS_TOKEN, genericAuthUser.AccessToken);

            // Get current identity for the user.
            ClaimsIdentity identity = await _access.GetClaimsIdentityAsync(genericAuthUser);

            // Associate the identity with a principal.
            ClaimsPrincipal principal = new(identity);

            // Notify the authentication about changes.
            NotifyAuthenticationStateChanged(principal);
        }

        /// <summary>
        /// Mark user as logged out.
        /// </summary>
        /// <returns>A notification event to the authentication state.</returns>
        public async Task MarkUserAsLoggedOut()
        {
            // Remove access token from the memory.
            await _localStorageService.RemoveItemAsync(ACCESS_TOKEN);

            // Initialize new identity.
            ClaimsIdentity identity = new();

            // Remove current associated data in the principal.
            ClaimsPrincipal user = new(identity);

            // Notify the authentication state about changes.
            NotifyAuthenticationStateChanged(user);
        }

        /// <summary>
        /// Notify the authentication for new changes.
        /// </summary>
        public void NotifyAuthenticationStateChanged(ClaimsPrincipal user)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
