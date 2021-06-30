using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class ChangePassword
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }

        [Inject] public UserService _userService { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private string infoMessage = string.Empty;
        private string errorMessage = string.Empty;
        private bool isProcessingPasswordChangeRequest = false;

        private ChangePasswordModel changePasswordModel;
        private ClaimsPrincipal claim;

        protected async override Task OnInitializedAsync()
        {
            changePasswordModel = new();

            var authState = await AuthState;

            if (authState == null)
            {
                errorMessage = "Noget gik galt da siden blev indlæst.";
                return;
            }

            claim = authState.User;

            if (claim == null)
            {
                errorMessage = "Noget gik galt, prøv at genindlæs siden.";
                return;
            }

            changePasswordModel.Email = claim.FindFirstValue(ClaimTypes.Email);
        }

        private async Task OnValidSubmit_ChangeUserPassword()
        {
            ClearMessages();

            try
            {
                isProcessingPasswordChangeRequest = true;

                infoMessage = "Arbejder på det, vent venligst..";

                Role role = null;
                Location location = null;
                List<Area> areaList = new List<Area>();

                User user = new(0, role, location, areaList, string.Empty, string.Empty, changePasswordModel.Email, changePasswordModel.Password);

                await _userService.UpdateUserPasswordAsync(user, _tokenSource.Token);

                infoMessage = "Adgangskode blev ændret.";
            }
            catch (Exception)
            {
                errorMessage = "Kunne ikke ændre adgangskode, prøv igen senere.";
            }
            finally
            {
                ResetModelInformation();

                isProcessingPasswordChangeRequest = false;
            }
        }

        private void OnInvalidSubmit_ChangeUserPassword()
        {
            errorMessage = "Venligst udfyld de manglende felter, markeret med rød.";
        }

        private void ResetModelInformation()
        {
            changePasswordModel = new();

            changePasswordModel.Email = claim.FindFirstValue(ClaimTypes.Email);
        }

        private void ClearMessages()
        {
            infoMessage = string.Empty;
            errorMessage = string.Empty;
        }
    }
}
