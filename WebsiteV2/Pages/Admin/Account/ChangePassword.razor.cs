using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services;
using BlazorServerWebsite.Data.Services.Abstractions;
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
        [Inject] public IUserService _userService { get; set; }
        [Inject] protected IMessageClearProvider MessageClearProvider { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private bool isProcessingPasswordChangeRequest = false;
        private string _infoMessage = string.Empty;
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _sessionUserEmail = string.Empty;
        private bool _userEmailAlreadyExists = false;
        private bool _isLoadingData = false;
        private bool _hasValidSession = true;
        private bool _isProcessingRequest = false;

        private ChangePasswordModel changePasswordModel;
        private ClaimsPrincipal claim;

        protected async override Task OnInitializedAsync()
        {
            changePasswordModel = new();
            _isLoadingData = true;

            var session = await AuthState;

            foreach (var sessionClaim in session.User.Claims)
            {
                if (sessionClaim.Type == ClaimTypes.Email)
                {
                    _sessionUserEmail = sessionClaim.Value;
                }
            }
            
            _isLoadingData = false;

            if (string.IsNullOrEmpty(_sessionUserEmail))
            {
                _errorMessage = "Fejl, Kunne ikke indlæse session. Prøv at logge ud og ind.";
                _hasValidSession = false;
            }

            await MessageClearProvider.ClearMessageOnIntervalAsync(_errorMessage);

            changePasswordModel.Email = claim.FindFirstValue(ClaimTypes.Email);

            await base.OnInitializedAsync();
        }


        private async Task OnValidSubmit_ChangeUserPassword()
        {
            ClearMessages();

            try
            {
                isProcessingPasswordChangeRequest = true;

                _infoMessage = "Arbejder på det, vent venligst..";

                Role role = null;
                Location location = null;
                List<Area> areaList = new List<Area>();

                User user = new(0, role, location, areaList, string.Empty, string.Empty, changePasswordModel.Email, changePasswordModel.Password);

                await _userService.UpdateUserPasswordAsync(user, _tokenSource.Token);

                _infoMessage = "Adgangskode blev ændret.";
            }
            catch (Exception)
            {
                _errorMessage = "Kunne ikke ændre adgangskode, prøv igen senere.";
            }
            finally
            {
                ResetModelInformation();

                isProcessingPasswordChangeRequest = false;
            }
        }

        private void OnInvalidSubmit_ChangeUserPassword()
        {
            _errorMessage = "Venligst udfyld de manglende felter, markeret med rød.";
        }

        private void ResetModelInformation()
        {
            changePasswordModel = new();

            changePasswordModel.Email = claim.FindFirstValue(ClaimTypes.Email);
        }

        private void ClearMessages()
        {
            _infoMessage = string.Empty;
            _errorMessage = string.Empty;
        }
    }
}
