using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Account
{
    /// <summary>
    /// A partial class of User Profile Component View.
    /// </summary>
    public partial class UserProfileComponent : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        [Inject] protected MyAuthStateProvider MyAuthStateProvider { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        private readonly string _roleClaimType = ClaimTypes.Role;
        private readonly string _locationClaimType = "Location";
        private readonly string _consultantAreaClaimType = "ConsultantArea";
        private AuthenticationState _context;

        protected override void OnInitialized()
        {
            _context = AuthenticationState.Result;

            //MyAuthStateProvider.AuthenticationStateChanged += MyAuthStateProvider_AuthenticationStateChanged;
            MyAuthStateProvider.AuthenticationStateChanged += async (authStateTask) =>
            {
                _context = await MyAuthStateProvider.GetAuthenticationStateAsync();

                StateHasChanged();
                base.ShouldRender();
            };

            base.OnInitialized();
        }

        private async Task LogOut()
        {
            await (MyAuthStateProvider.MarkUserAsLoggedOut());

            NavigationManager.NavigateTo("/", true);
        }
    }
}
