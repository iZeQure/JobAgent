using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using JobAgent.Data.Interfaces;

namespace JobAgent.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService LocalStorageService { get; }
        public IUserService UserSerivce { get; set; }

        public CustomAuthenticationStateProvider(
            ILocalStorageService localStorageService,
            IUserService userService)
        {
            LocalStorageService = localStorageService;
            UserSerivce = userService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var accessToken = await LocalStorageService.GetItemAsync<string>("accessToken");

            ClaimsIdentity identity;

            if (accessToken != null && accessToken != string.Empty)
            {
                User user = await UserSerivce.GetUserByAccessToken(accessToken);
                identity = GetClaimsIdentity(user);
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var claimsPrincipal = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task MarkUserAuthenticated(User user)
        {
            await LocalStorageService.SetItemAsync("accessToken", user.AccessToken);
            await LocalStorageService.SetItemAsync("refreshToken", user.RefreshToken);

            var identity = GetClaimsIdentity(user);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public async Task MarkUserAsLoggedOut()
        {
            await LocalStorageService.RemoveItemAsync("refreshToken");
            await LocalStorageService.RemoveItemAsync("accessToken");

            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            var claimsIdentity = new ClaimsIdentity();

            if (user.Email != null)
            {
                claimsIdentity = new ClaimsIdentity(new[]
                                {
                                    new Claim("UserId", $"{user.Id}"),
                                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                                    new Claim(ClaimTypes.Email, user.Email),
                                    new Claim(ClaimTypes.Role, user.ConsultantArea.Name)
                                }, "serverauth_type");
            }

            return claimsIdentity;
        }
    }
}
