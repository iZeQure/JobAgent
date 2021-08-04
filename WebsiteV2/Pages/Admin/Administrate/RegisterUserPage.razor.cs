using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class RegisterUser : ComponentBase
    {
        [Inject] private IUserService UserService { get; set; }
        [Inject] private IAreaService AreaService { get; set; }
        [Inject] private ILocationService LocationService { get; set; }

        private RegisterUserModel _regAccModel;
        private User User;
        private IEnumerable<Area> _areas;
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
            _areas = new List<Area>();
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
                _areas = await AreaService.GetAllAsync(_tokenSource.Token);
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

                var userService = (UserService)UserService;

                var userResult = await userService.CreateAsync(User, _tokenSource.Token);
                await userService.GrantUserAreaAsync(User, _regAccModel.ConsultantAreaId, _tokenSource.Token);

                infoMessage = "Praktikkonsulent blev oprettet succesfuldt";
            }
            catch (Exception)
            {
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
