using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Security.Cryptography;
using JobAgentClassLibrary.Security.interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Administrate
{
    public partial class RegisterUserPage : ComponentBase
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IRoleService RoleService { get; set; }
        [Inject] private ILocationService LocationService { get; set; }
        [Inject] private IAuthenticationAccess AuthenticationAccess { get; set; }

        private RegisterUserModel _regAccModel;
        private IEnumerable<IRole> _roles;
        private IEnumerable<ILocation> _locations;

        private bool isProcessingRequest = false;
        private bool dataIsLoading = false;
        private bool errorOcurred = false;
        private bool emailExists = false;
        private string errorMessage = string.Empty;
        private string infoMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _roles = new List<IRole>();
            _locations = new List<ILocation>();
            _regAccModel = new();

            await LoadInformation();
        }

        private async Task LoadInformation()
        {
            errorOcurred = false;
            dataIsLoading = true;

            try
            {
                _roles = await RoleService.GetRolesAsync();
                _locations = await LocationService.GetLocationsAsync();
            }
            catch (Exception)
            {
                errorOcurred = true;
                errorMessage = "Kunne ikke hente nødvendig information, prøv igen senere.";
            }
            finally
            {
                dataIsLoading = false;
            }
        }

        private async Task OnValidSubmit_RegisterAccountAsync()
        {
            ClearMessages();

            errorOcurred = false;
            isProcessingRequest = true;

            try
            {
                if (emailExists)
                {
                    CancelRegistrationRequest();
                    errorMessage = "Denne email er allerede i brug";
                    return;
                }

                AuthUser tempUser = new AuthUser
                {
                    Id = 0,
                    RoleId = _regAccModel.RoleId,
                    LocationId = _regAccModel.LocationId,
                    FirstName = _regAccModel.FirstName,
                    LastName = _regAccModel.LastName,
                    Email = _regAccModel.Email,
                    Password = _regAccModel.Password

                };

                tempUser.GenerateSalt();
                tempUser.HashPassword();

                string accToken = await AuthenticationAccess.GenerateAccessTokenAsync(tempUser);

                IUser user = new AuthUser
                {
                    Id = tempUser.Id,
                    RoleId = tempUser.RoleId,
                    LocationId = tempUser.LocationId,
                    FirstName = tempUser.FirstName,
                    LastName = tempUser.LastName,
                    Email = tempUser.Email,
                    Password = tempUser.Password,
                    Salt = tempUser.Salt,
                    AccessToken = accToken
                };

                var userResult = await UserService.CreateAsync(user);
                bool userIsRegistered = false;

                if (userResult.FirstName == user.FirstName && userResult.Email == user.Email)
                {
                    userIsRegistered = true;
                }

                if (userIsRegistered)
                {
                    errorMessage = "Brugeren blev ikke oprettet, der skete en fejl. Prøv igen senere.";
                    return;
                }


                infoMessage = "Praktikkonsulent blev oprettet succesfuldt";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                errorOcurred = true;
                errorMessage = "Ukendt Fejl.";
            }
            finally
            {
                isProcessingRequest = false;
                _regAccModel = new();
            }
        }

        private async Task OnEmailFocusOut_CheckForExistence(FocusEventArgs e)
        {
            try
            {
                var userService = (UserService)UserService;

                if (userService != null)
                {
                    if (string.IsNullOrEmpty(_regAccModel.Email) || string.IsNullOrWhiteSpace(_regAccModel.Email))
                    {
                        emailExists = false;
                        return;
                    }

                    emailExists = await userService.CheckUserExistsAsync(_regAccModel.Email);
                }
            }
            catch (Exception)
            {
                errorMessage = "Fejl ved tjek om email findes.";
            }
        }

        private void CancelRegistrationRequest()
        {
            errorMessage = "Praktikkonsulent eksisterer allerede.";
            _regAccModel.Email = string.Empty;
        }

        private void ClearMessages()
        {
            infoMessage = string.Empty;
            errorMessage = string.Empty;
        }
    }

    public class RegisterUserModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst fornavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst efternavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255, ErrorMessage = "Adgangskode er for lang eller kort.", MinimumLength = 6)]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Genindtast din adgangskode.")]
        [Compare(nameof(Password), ErrorMessage = "Adgangskode stemmer ikke overens.")]
        [StringLength(255, ErrorMessage = "Adgangskode er for lang eller kort.", MinimumLength = 6)]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en Rolle.")]
        public int RoleId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en lokation.")]
        public int LocationId { get; set; }
    }

}
