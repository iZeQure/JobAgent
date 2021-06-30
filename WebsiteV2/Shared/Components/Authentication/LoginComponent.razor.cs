using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using System.Threading;

namespace BlazorServerWebsite.Shared.Components.Authentication
{
    public partial class LoginComponent : ComponentBase
    {
        [Inject] protected MyAuthStateProvider MyAuthStateProvider { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected IUserService UserService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private IUser user;
        private EditContext _editContext;
        private AuthenticationModel _authenticationModel = new();
        private bool _processingRequest = false;
        private string _message = string.Empty;

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_authenticationModel);
            _editContext.AddDataAnnotationsValidation();
        }

        private async Task OnValidSubmit_LogInAsync()
        {
            _processingRequest = true;

            _message = "Logger ind, vent venligst..";

            if (_authenticationModel.IsValidEmail(_authenticationModel.Email))
            {
                user = new User(
                    0, null, null, null, "", "",
                    _authenticationModel.Email,
                    _authenticationModel.Password);

                // Get the task to load.
                var loginTask = UserService.LoginAsync(user, _tokenSource.Token);

                // Await the task to finish.
                await Task.WhenAll(loginTask);

                if (loginTask.Result)
                {
                    await MyAuthStateProvider.MarkUserAsAuthenticated(user);

                    NavigationManager.NavigateTo("Admin", true);
                }
                else
                {
                    _message = "Email eller adgangskode er forkert.";
                    _processingRequest = false;
                }
            }
            else
            {
                _message = "Email eller adgangskode er forkert.";
                _processingRequest = false;
            }

            await ClearValidationMessageAfterInterval();
        }

        private Task ClearValidationMessageAfterInterval(int milliseconds = 3000)
        {
            var x = Task.Delay(milliseconds)
                .ContinueWith(x =>
                {
                    _message = string.Empty;
                    return x.IsCompleted;
                });

            return x;
        }
    }
}
