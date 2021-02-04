using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using JobAgent.Data.Interfaces;
using Blazored.LocalStorage;
using Pocos;

namespace JobAgent.Data.Security
{
    public class MyAuthStateProvider : AuthenticationStateProvider
    {
        private ILocalStorageService LocalStorageService;
        private IUserService UserService;

        /// <summary>
        /// Access the value for the access token in the memory.
        /// </summary>
        private const string ACCESS_TOKEN = "AccessToken";

        public static bool IsAuthenticated { get; set; }
        public static bool IsAuthenticating { get; set; }

        public MyAuthStateProvider()
        {

        }

        public MyAuthStateProvider(ILocalStorageService localStorageService, IUserService userService)
        {
            LocalStorageService = localStorageService;
            UserService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Declare a variable to store the identity.
            ClaimsIdentity identity;

            // Declare a varible to store the user.
            User user;

            // Get the access token, from current auth state.
            string accessToken = await LocalStorageService.GetItemAsync<string>(ACCESS_TOKEN);

            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(accessToken))
            {
                // Get the user by access token.
                user = await UserService.GetUserByAccessToken(accessToken);

                // TODO : Call get claims identity, to store the user into.
                // Get claims identity with the authenticated user.
                identity = GetClaimsIdentity(user);
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
        public async Task MarkUserAsAuthenticated(User user)
        {
            // Set the access token in the local memory.
            await LocalStorageService.SetItemAsync(ACCESS_TOKEN, user.AccessToken);

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
            await LocalStorageService.RemoveItemAsync(ACCESS_TOKEN);

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
        public ClaimsIdentity GetClaimsIdentity(User user)
        {
            // Initialize new identity.
            ClaimsIdentity identity = new ClaimsIdentity();

            // Check if the object isn't initialized.
            if (user != null)
            {
                // Check if the obj is correct.
                if (user.Identifier != 0)
                {
                    identity = new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim("UserId", $"{user.Identifier}"),
                                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                                    new Claim(ClaimTypes.Email, user.Email),
                                    new Claim(ClaimTypes.Role, user.ConsultantArea.Name),
                                    new Claim("LocationName", $"{user.Location.Name}")
                                }, $"{user.AccessToken}");
                }
            }

            return identity;
        }
    }
}
