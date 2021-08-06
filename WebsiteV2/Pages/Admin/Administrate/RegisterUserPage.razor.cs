using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System;
using SecurityLibrary.Cryptography.Extentions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SecurityLibrary.Interfaces;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class RegisterUserPage : ComponentBase
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IRoleService RoleService { get; set; }
        [Inject] private ILocationService LocationService { get; set; }
        [Inject] private IAuthenticationAccess AuthenticationAccess { get; set; }

        private RegisterUserModel _regAccModel;
        private IEnumerable<Role> _roles;
        private IEnumerable<Location> _locations;
        private CancellationTokenSource _tokenSource = new();

        private bool isProcessingRequest = false;
        private bool dataIsLoading = false;
        private bool errorOcurred = false;
        private bool emailExists = false;
        private string errorMessage = string.Empty;
        private string infoMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _roles = new List<Role>();
            _locations = new List<Location>();
            _regAccModel = new();

            await LoadInformation();
        }

        private async Task LoadInformation()
        {
            errorOcurred = false;
            dataIsLoading = true;

            try
            {
                _roles = await RoleService.GetAllAsync(_tokenSource.Token);
                _locations = await LocationService.GetAllAsync(_tokenSource.Token);
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

                IUser tempUser = new User(
                    0,
                    new Role(_regAccModel.RoleId, ""),
                    new Location(_regAccModel.LocationId, ""),
                    null,
                    _regAccModel.FirstName,
                    _regAccModel.LastName,
                    _regAccModel.Email,
                    _regAccModel.Password
                    );
                tempUser.GenerateSalt();
                tempUser.HashPassword();

                string accToken = AuthenticationAccess.GenerateAccessToken(tempUser);

                IUser user = new User(
                    tempUser.Id,
                   tempUser.GetRole,
                    tempUser.GetLocation,
                    null,
                    tempUser.GetFirstName,
                    tempUser.GetLastName,
                    tempUser.GetEmail,
                    tempUser.GetPassword,
                    tempUser.GetSalt,
                    accToken
                    );
                

                var userResult = await UserService.CreateAsync(user, _tokenSource.Token);

                if(userResult == 0)
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

                    emailExists = await userService.ValidateUserExistsByEmail(_regAccModel.Email, _tokenSource.Token);
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
}
