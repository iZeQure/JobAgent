using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Account
{
    public partial class ChangePassword
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] protected IMessageClearProvider MessageClearProvider { get; set; }
        [Inject] public IUserService UserService { get; set; }

        private bool isProcessingPasswordChangeRequest = false;
        private string _sessionUserEmail = string.Empty;
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _infoMessage = string.Empty;
        private bool _hasValidSession = true;
        private bool _isLoadingData = false;

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

                IUser user = await UserService.GetByEmailAsync(_sessionUserEmail);


                IAuthUser authUser = new AuthUser
                {
                    Id = user.Id,
                    RoleId = user.RoleId,
                    LocationId = user.LocationId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = changePasswordModel.Password
                };

                await UserService.UpdateUserPasswordAsync(authUser);

                _infoMessage = "Adgangskode blev ændret.";

            }
            catch (Exception ex)
            {
                ClearMessages();
                _errorMessage = "Kunne ikke ændre adgangskode, prøv igen senere.";
                Console.WriteLine(ex.Message);
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
