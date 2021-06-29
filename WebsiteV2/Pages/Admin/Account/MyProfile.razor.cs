using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services.Abstractions;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System.Threading;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

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
                // Load Session Data.
                var session = await AuthenticationState;

                foreach (var sessionClaim in session.User.Claims)
                {
                    if (sessionClaim.Type == ClaimTypes.Email)
                    {
                        _sessionUserEmail = sessionClaim.Value;
                    }
                }

                // Load User Data.
                var user = await UserService.GetUserByEmailAsync(_sessionUserEmail, _tokenSource.Token);
                _accountProfileModel = new()
                {
                    AccountId = user.GetUserId,
                    FirstName = user.GetFirstName,
                    LastName = user.GetLastName,
                    Email = user.GetEmail
                };

                _locations = await LocationService.GetAllAsync(_tokenSource.Token);
                _areas = await AreaService.GetAllAsync(_tokenSource.Token);
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
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            if (UserService is IUserService service)
            {
                if (UserEmail != _accountProfileModel.Email)
                {
                    try
                    {
                        UserAlreadyExists = await service.ValidateUserExistsByEmail(_accountProfileModel.Email, token);
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
