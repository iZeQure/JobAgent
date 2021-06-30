using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System.Threading;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using SecurityLibrary.Providers;
using BlazorServerWebsite.Data.Providers;

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class MyProfile : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }
        [Inject] protected IAreaService AreaService { get; set; }
        [Inject] protected IRoleService RoleService { get; set; }
        [Inject] protected IMessageClearProvider MessageClearProvider { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private EditContext _editContext;
        private AccountProfileModel _accountProfileModel = new();
        private IEnumerable<Location> _locations = new List<Location>();
        private IEnumerable<Area> _areas = new List<Area>();
        private IEnumerable<Role> _roles = new List<Role>();
        private IEnumerable<Area> _assignedConsultantAreas = new List<Area>();

        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _sessionUserEmail = string.Empty;
        private bool _userEmailAlreadyExists = false;
        private bool _isLoadingData = false;
        private bool _hasValidSession = true;
        private bool _isProcessingRequest = false;

        protected override async Task OnInitializedAsync()
        {
            _isLoadingData = true;

            _editContext = new(_accountProfileModel);
            _editContext.AddDataAnnotationsValidation();

            // Load Session Data.
            var session = await AuthenticationState;

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

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(_sessionUserEmail))
                {
                    _isLoadingData = true;

                    // Get tasks to load.
                    var userTask = UserService.GetUserByEmailAsync(_sessionUserEmail, _tokenSource.Token);
                    var locationsTask = LocationService.GetAllAsync(_tokenSource.Token);
                    var areasTask = AreaService.GetAllAsync(_tokenSource.Token);
                    var roleTask = RoleService.GetAllAsync(_tokenSource.Token);

                    // Wait for data to be loaded.
                    try
                    {
                        await Task.WhenAll(userTask, locationsTask, areasTask, roleTask);

                        _locations = locationsTask.Result;
                        _areas = areasTask.Result;
                        _roles = roleTask.Result;
                        var user = userTask.Result;

                        _assignedConsultantAreas = user.GetConsultantAreas;
                        _accountProfileModel = new()
                        {
                            AccountId = user.GetUserId,
                            RoleId = user.GetRole.Id,
                            LocationId = user.GetLocation.Id,
                            FirstName = user.GetFirstName,
                            LastName = user.GetLastName,
                            Email = user.GetEmail
                        };

                        _isLoadingData = false;
                    }
                    catch (Exception ex)
                    {
                        _isLoadingData = false;
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnValidSubmit_ChangeUserInformation()
        {
            if (_hasValidSession)
            {
                _isProcessingRequest = true;

                IUser user = new User(
                    _accountProfileModel.AccountId,
                    new Role(_accountProfileModel.RoleId, "", ""),
                    new Location(_accountProfileModel.LocationId, ""),
                    null,
                    _accountProfileModel.FirstName,
                    _accountProfileModel.LastName,
                    _accountProfileModel.Email);

                int result = await UserService.UpdateAsync(user, _tokenSource.Token);

                if (result == 1)
                {
                    _successMessage = "Din bruger blev opdateret!";
                    await MessageClearProvider.ClearMessageOnIntervalAsync(_successMessage);
                    return;
                }

                _errorMessage = "Fejl, noget gik galt ved opdatering af din bruger.";
                await MessageClearProvider.ClearMessageOnIntervalAsync(_errorMessage);
            }
        }

        private async Task OnFocusOut_CheckUserAlreadyExists(FocusEventArgs e)
        {
            if (UserService is IUserService service)
            {
                if (string.IsNullOrEmpty(_sessionUserEmail))
                {
                    Console.WriteLine($"No Session. Cannot validate user email.");

                    return;
                }

                if (_sessionUserEmail != _accountProfileModel.Email)
                {
                    if (string.IsNullOrEmpty(_accountProfileModel.Email))
                    {
                        Console.WriteLine("No validation made, email was not supplied.");
                        return;
                    }

                    try
                    {
                        _userEmailAlreadyExists = await service.ValidateUserExistsByEmail(_accountProfileModel.Email, _tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Console.WriteLine("Email is not modified.");
            }
        }
    }
}
