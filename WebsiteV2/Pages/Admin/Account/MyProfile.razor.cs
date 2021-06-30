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

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class MyProfile : ComponentBase
    {
        [CascadingParameter] protected Task<AuthenticationState> AuthenticationState { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }
        [Inject] protected IAreaService AreaService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private EditContext _editContext;
        private AccountProfileModel _accountProfileModel = new();
        private IEnumerable<Location> _locations = new List<Location>();
        private IEnumerable<Area> _areas = new List<Area>();

        private string _processMessage = string.Empty;
        private string _sessionUserEmail = string.Empty;
        private bool _userEmailAlreadyExists = false;
        private bool _isLoadingData = false;

        protected override Task OnInitializedAsync()
        {
            _editContext = new(_accountProfileModel);
            _editContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isLoadingData = true;

                // Load Session Data.
                var session = await AuthenticationState;

                foreach (var sessionClaim in session.User.Claims)
                {
                    if (sessionClaim.Type == ClaimTypes.Email)
                    {
                        _sessionUserEmail = sessionClaim.Value;
                    }
                }

                // Get tasks to load.
                var userTask = UserService.GetUserByEmailAsync(_sessionUserEmail, _tokenSource.Token);
                var locationsTask = LocationService.GetAllAsync(_tokenSource.Token);
                var areasTask = AreaService.GetAllAsync(_tokenSource.Token);

                // Wait for data to be loaded.
                try
                {
                    await Task.WhenAll(userTask, locationsTask, areasTask);

                    _locations = locationsTask.Result;
                    _areas = areasTask.Result;
                    var user = userTask.Result;

                    _accountProfileModel = new()
                    {
                        AccountId = user.GetUserId,
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

            await base.OnAfterRenderAsync(firstRender);
        }

        private Task OnValidSubmit_ChangeUserInformation()
        {
            if (_accountProfileModel.AccountId == 0)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        private async Task OnFocusOut_CheckUserAlreadyExists(FocusEventArgs e)
        {
            if (UserService is IUserService service)
            {
                if (_sessionUserEmail != _accountProfileModel.Email)
                {
                    try
                    {
                        _userEmailAlreadyExists = await service.ValidateUserExistsByEmail(_accountProfileModel.Email, _tokenSource.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }

                }
            }
            
        }

    }
}
