using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using static BlazorWebsite.Shared.Components.Diverse.MessageAlert;

namespace BlazorWebsite.Pages.Dashboard.Account
{
    public partial class ChangePassword
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] protected MessageClearProvider MessageClearProvider { get; set; }
        [Inject] public IUserService UserService { get; set; }

        private AlertType alertType;
        private bool isProcessingPasswordChangeRequest = false;
        private string _sessionUserEmail = string.Empty;
        private string message = string.Empty;
        
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
                message = "Fejl, Kunne ikke indlæse session. Prøv at logge ud og ind.";
                alertType = AlertType.Error;
            }

            changePasswordModel.Email = _sessionUserEmail;
        }


        private async Task OnValidSubmit_ChangeUserPasswordAsync()
        {
            try
            {
                isProcessingPasswordChangeRequest = true;

                message = "Arbejder på det, vent venligst..";
                alertType = AlertType.Info;

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

                message = "Adgangskode blev ændret.";

            }
            catch (Exception ex)
            {
                message = "Kunne ikke ændre adgangskode, prøv igen senere.";
                alertType= AlertType.Error;
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
            message = "Venligst udfyld de manglende felter, markeret med rød.";
            alertType = AlertType.Warning;
        }

        private void ResetModelInformation()
        {
            changePasswordModel = new();

            changePasswordModel.Email = _sessionUserEmail;
        }
       
    }
}
