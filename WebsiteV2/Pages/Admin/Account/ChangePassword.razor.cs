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
using SecurityLibrary.Cryptography.Extentions;
using System.Diagnostics;

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class ChangePassword
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] protected IMessageClearProvider MessageClearProvider { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private bool isProcessingPasswordChangeRequest = false;
        private string _infoMessage = string.Empty;
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _sessionUserEmail = string.Empty;
        private bool _isLoadingData = false;
        private bool _hasValidSession = true;
        private bool _isProcessingRequest = false;

        private ChangePasswordModel changePasswordModel;

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

            changePasswordModel.Email = _sessionUserEmail;

            await base.OnInitializedAsync();
        }


        private async Task OnValidSubmit_ChangeUserPassword()
        {
            ClearMessages();

            try
            {
                isProcessingPasswordChangeRequest = true;

                _infoMessage = "Arbejder på det, vent venligst..";

                IUser user = await UserService.GetUserByEmailAsync(_sessionUserEmail, _tokenSource.Token);

                user.SetPassword(changePasswordModel.Password);

                user.GenerateSalt();
                user.HashPassword();

                await UserService.UpdateUserPasswordAsync(user, _tokenSource.Token);

                _infoMessage = "Adgangskode blev ændret.";
            }
            catch (Exception)
            {
                ClearMessages();
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

            changePasswordModel.Email = _sessionUserEmail;
        }

        private void ClearMessages()
        {
            _infoMessage = string.Empty;
            _errorMessage = string.Empty;
        }
    }
}
