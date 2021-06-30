using Blazored.LocalStorage;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;
using ObjectLibrary.Common;
using SecurityLibrary.Access;
using SecurityLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Providers
{
    public class MyAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IUserService _userService;
        private readonly IAuthenticationAccess _access;

        /// <summary>
        /// Access the value for the access token in the memory.
        /// </summary>
        private const string ACCESS_TOKEN = "AccessToken";

        public static bool IsAuthenticated { get; set; }
        public static bool IsAuthenticating { get; set; }

        public MyAuthStateProvider()
        {

        }

        public MyAuthStateProvider(ILocalStorageService localStorageService, IUserService userService, IAuthenticationAccess userAccess)
        {
            _localStorageService = localStorageService;
            _userService = userService;
            _access = userAccess;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            CancellationTokenSource tokenSource = new();
            

            // Declare a variable to store the identity.
            ClaimsIdentity identity;

            // Declare a varible to store the user.
            IUser user;

            // Get the access token, from current auth state.
            string accessToken = await _localStorageService.GetItemAsync<string>(ACCESS_TOKEN);

            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(accessToken))
            {
                // Get the user by access token.
                user = await _userService.GetUserByAccessTokenAsync(accessToken, tokenSource.Token);

                // TODO : Call get claims identity, to store the user into.
                // Get claims identity with the authenticated user.
                identity = _access.GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            ClaimsPrincipal principalUser = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(principalUser));
        }

        /// <summary>
        /// Mark user as authenticated, if credentials is valid from server side.
        /// </summary>
        /// <param name="user">Used to authenticate the current user.</param>
        /// <returns>A authentication notification task changed.</returns>
        public async Task MarkUserAsAuthenticated(IUser user)
        {
            // Set the access token in the local memory.
            await _localStorageService.SetItemAsync(ACCESS_TOKEN, user.GetAccessToken);

            // Get current identity for the user.
            ClaimsIdentity identity = GetClaimsIdentity(user);

            // Associate the identity with a principal.
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

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
            ClaimsIdentity identity = new ClaimsIdentity();

            // Remove current associated data in the principal.
            ClaimsPrincipal user = new ClaimsPrincipal(identity);

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

        /// <summary>
        /// Get claims identity access.
        /// </summary>
        /// <param name="user">Used to express to definition of the claims identity.</param>
        /// <returns>An identity with the user.</returns>
        public ClaimsIdentity GetClaimsIdentity(IUser user)
        {
            return _access.GetClaimsIdentity(user);
        }
    }
}
