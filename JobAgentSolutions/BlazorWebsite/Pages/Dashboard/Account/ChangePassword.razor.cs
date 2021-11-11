using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using JobAgentClassLibrary.Security.Cryptography;

namespace BlazorWebsite.Pages.Dashboard.Account
{
    public partial class ChangePassword
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] public IUserService UserService { get; set; }
        [Inject] protected IMessageClearProvider MessageClearProvider { get; set; }

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

                IUser user = await UserService.GetByEmailAsync(_sessionUserEmail);

                if (user is AuthUser authUser)
                {
                    authUser.Password = changePasswordModel.Password;

                    await UserService.UpdateUserPasswordAsync((IAuthUser)user);

                    _infoMessage = "Adgangskode blev ændret.";
                }
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
    public class ChangePasswordModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(255, ErrorMessage = "Email er for long (255 karakter begrænse.).")]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Genindtast din adgangskode.")]
        [StringLength(255)]
        [Compare(nameof(Password), ErrorMessage = "Adgangskode stemmer ikke overens.")]
        public string ConfirmPassword { get; set; }
    }
}
