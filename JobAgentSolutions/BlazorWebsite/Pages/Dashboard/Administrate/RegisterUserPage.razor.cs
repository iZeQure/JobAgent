using BlazorWebsite.Data.FormModels;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Settings;
using JobAgentClassLibrary.Security.Access;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class RegisterUserPage : ComponentBase
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IRoleService RoleService { get; set; }
        [Inject] private ILocationService LocationService { get; set; }
        [Inject] private ISecuritySettings SecuritySettings { get; set; }

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

            UserAccess userAccess = new UserAccess(SecuritySettings, LocationService, RoleService);

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
                    Password = _regAccModel.Password,

                };

                tempUser.AccessToken = await userAccess.GenerateAccessTokenAsync(tempUser);

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
                    AccessToken = tempUser.AccessToken
                };

                var userResult = await UserService.CreateAsync(user);
                bool userIsRegistered = false;

                if (userResult.FirstName == user.FirstName && userResult.Email == user.Email)
                {
                    userIsRegistered = true;
                }

                if (!userIsRegistered)
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
                ClearMessages();
                var userService = (UserService)UserService;

                if (userService != null)
                {
                    if (string.IsNullOrEmpty(_regAccModel.Email) || string.IsNullOrWhiteSpace(_regAccModel.Email))
                    {
                        emailExists = false;
                        return;
                    }

                    emailExists = await userService.CheckUserExistsAsync(_regAccModel.Email);

                    if (emailExists)
                    {
                        errorMessage = "Denne mail er allerede i systemet.";
                    }
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
