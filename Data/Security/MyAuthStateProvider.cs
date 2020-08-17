using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using JobAgent.Data.Interfaces;
using System.Collections.Generic;

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

        // Test object, leave for now. - Out comment if necessary .
        public User User { get; set; } = new User();

        public MyAuthStateProvider(ILocalStorageService localStorageService, IUserService userService)
        {
            LocalStorageService = localStorageService;
            UserService = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Declare a variable to store the identity.
            ClaimsIdentity identity;

            // Get the access token, from current auth state.
            string accessToken = await LocalStorageService.GetItemAsync<string>(ACCESS_TOKEN);

            if (!string.IsNullOrWhiteSpace(accessToken) && !string.IsNullOrEmpty(accessToken))
            {
                // Get the user by access token.
                User = await UserService.GetUserByAccessToken(accessToken);

                // TODO : Call get claims identity, to store the user into.
                // Get claims identity with the authenticated user.
                identity = GetClaimsIdentity(User);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));           
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
        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            // Initialize new identity.
            ClaimsIdentity identity = new ClaimsIdentity();

            // Check if the object isn't initialized.
            if (user != null)
            {
                // Check if the obj is correct.
                if (user.Id != 0)
                {
                    identity = new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim("UserId", $"{User.Id}"),
                                    new Claim(ClaimTypes.Name, $"{User.FirstName} {User.LastName}"),
                                    new Claim(ClaimTypes.Email, User.Email),
                                    new Claim(ClaimTypes.Role, User.ConsultantArea.Name),
                                    new Claim("LocationName", $"{User.Location.Name}")
                                }, "LOCAL_AUTH");
                }
            }

            return identity;
        }
    }
}
