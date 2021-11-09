using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Areas;
using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Admin.Account
{
    public partial class MyProfile : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        [Inject] protected MyAuthStateProvider MyAuthStateProvider { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }
        [Inject] protected IAreaService AreaService { get; set; }
        [Inject] protected IRoleService RoleService { get; set; }

        private readonly AccountProfileDiverseModel _accountProfileDiverseModel = new();
        private IUser _userSession;
        private EditContext _editContext;
        private AccountProfileModel _accountProfileModel = new();

        private IEnumerable<Location> _locations = new List<Location>();
        private IEnumerable<Area> _areas = new List<Area>();
        private IEnumerable<Role> _roles = new List<Role>();
        private IEnumerable<Area> _assignedConsultantAreas = new List<Area>();

        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;
        private string _sessionUserEmail = string.Empty;
        private bool _hasValidSession = true;
        private bool _userEmailAlreadyExists = false;
        private bool _isLoadingData = false;
        private bool _isProcessingUpdateUserRequest = false;
        private bool _isProcessingUpdateConsultantAreaRequest = false;

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_accountProfileModel);
            _editContext.AddDataAnnotationsValidation();
        }

        protected override async Task OnInitializedAsync()
        {
            _isLoadingData = true;
            try
            {
                // Load Session Data.
                var session = await AuthenticationState;

                foreach (var sessionClaim in session.User.Claims)
                {
                    if (sessionClaim.Type == ClaimTypes.Email)
                    {
                        _sessionUserEmail = sessionClaim.Value;
                    }
                }

                if (string.IsNullOrEmpty(_sessionUserEmail))
                {
                    _errorMessage = "Fejl, Kunne ikke indlæse session. Prøv at logge ud og ind.";
                    _hasValidSession = false;
                }

                await LoadingData();
            }
            finally
            {
                _isLoadingData = false;
            }
        }

        private async Task LoadingData()
        {
            if (!string.IsNullOrEmpty(_sessionUserEmail))
            {
                // Get tasks to load.
                var userTask = UserService.GetByEmailAsync(_sessionUserEmail);
                var locationsTask = LocationService.GetLocationsAsync();
                var areasTask = AreaService.GetAllAsync();
                var roleTask = RoleService.GetRolesAsync();

                // Wait for data to be loaded.
                try
                {
                    await Task.WhenAll(userTask, locationsTask, areasTask, roleTask);

                    _locations = (IEnumerable<Location>)locationsTask.Result;
                    _areas = (IEnumerable<Area>)areasTask.Result;
                    _roles = (IEnumerable<Role>)roleTask.Result;
                    _userSession = userTask.Result;

                    if (_userSession == null)
                    {
                        _errorMessage = "Fejl, kunne ikke indlæse session.";
                        return;
                    }

                    _assignedConsultantAreas = (IEnumerable<Area>)_userSession.ConsultantAreas;
                    _accountProfileModel = new()
                    {
                        RoleId = _userSession.GetRole.Id,
                        LocationId = _userSession.GetLocation.Id,
                        FirstName = _userSession.GetFirstName,
                        LastName = _userSession.GetLastName,
                        Email = _userSession.GetEmail
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task OnValidSubmit_ChangeUserInformation()
        {
            Console.WriteLine($"Form Submitted");

            if (_hasValidSession)
            {
                _isProcessingUpdateUserRequest = true;

                IUser user = new User()
                {
                    Id = _userSession.Id,
                    RoleId = _accountProfileModel.RoleId,
                    LocationId = _accountProfileModel.LocationId,
                    FirstName = _accountProfileModel.FirstName,
                    LastName = _accountProfileModel.LastName,
                    Email = _accountProfileModel.Email
                };

                int result = await UserService.UpdateAsync(user);

                if (result == 1)
                {
                    _successMessage = "Din bruger blev opdateret!";
                    return;
                }

                _errorMessage = "Fejl, noget gik galt ved opdatering af din bruger.";
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
                        _userEmailAlreadyExists = await service.ValidateUserExistsByEmail(_accountProfileModel.Email);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    return;
                }
                Console.WriteLine("Email is not modified.");
                _userEmailAlreadyExists = false;
            }
        }

        private async Task OnButtonClick_AssignUserConsultantArea(int selectedAreaId)
        {
            try
            {
                _isProcessingUpdateConsultantAreaRequest = true;

                if (selectedAreaId == 0)
                {
                    _errorMessage = "Fejl, vælg venligst et område fra listen, før du trykker tilføj.";
                    return;
                }

                if (_assignedConsultantAreas.Any(x => x.Id == selectedAreaId))
                {
                    _errorMessage = "Kunne ikke tilføje konsulentområde; allerede tilføjet.";
                    return;
                }

                try
                {
                    var tempList = _assignedConsultantAreas.ToList();
                    var getAreaById = _areas.FirstOrDefault(x => x.Id == selectedAreaId);

                    int result = await UserService.GrantAreaToUserAsync(_userSession, selectedAreaId);

                    if (result == 1)
                    {
                        tempList.Add(getAreaById);
                        _assignedConsultantAreas = tempList.OrderBy(x => x.Id);
                        _successMessage = "Konsulentområde blev tilføjet!";
                        MyAuthStateProvider.NotifyAuthenticationStateChanged((await AuthenticationState).User);

                        return;
                    }

                    _errorMessage = "Der skete en uventet fejl ved tilføjelse af konsulentområdet.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            finally
            {
                _isProcessingUpdateConsultantAreaRequest = false;
            }
        }

        private async Task OnButtonClick_RemoveUserConsultantArea(int selectedAreaId)
        {
            try
            {
                _isProcessingUpdateConsultantAreaRequest = true;

                if (selectedAreaId == 0)
                {
                    _errorMessage = "Fejl, vælg venligst et område fra listen, for at fjerne det.";
                    return;
                }

                if (_assignedConsultantAreas.Count() == 1)
                {
                    _errorMessage = "Fejl, kan ikke fjerne alle områder, der skal minimum være et.";
                    return;
                }

                try
                {
                    var areaToBeRemoved = _areas.FirstOrDefault(x => x.Id == selectedAreaId);

                    int result = (await UserService.RevokeAreaFromUserAsync(_userSession, areaToBeRemoved.Id)).FirstName.Length >= 1;

                    if (result == 1)
                    {
                        var areas = _assignedConsultantAreas.ToList();
                        areas.RemoveAll(x => x.Name.Equals(areaToBeRemoved.Name));

                        _assignedConsultantAreas = areas;
                        _successMessage = "Konsulentområde blev fjernet!";
                        MyAuthStateProvider.NotifyAuthenticationStateChanged((await AuthenticationState).User);

                        return;
                    }

                    _errorMessage = "Der skete en uventet fejl ved fjernelse af konsulentområdet.";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            finally
            {
                _isProcessingUpdateConsultantAreaRequest = false;
            }
        }

        private void Test()
        {
            Console.WriteLine($"Form Invalid.");
        }
    }
}
