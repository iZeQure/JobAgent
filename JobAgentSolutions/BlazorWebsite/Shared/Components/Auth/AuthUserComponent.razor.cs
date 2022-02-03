using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Factory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Auth
{
    public partial class AuthUserComponent : ComponentBase
    {
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected UserEntityFactory UserFactory { get; set; }
        [Inject] protected MyAuthStateProvider MyAuthStateProvider { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        private EditContext _formContext;
        private AuthUserModel _authUserModel = new();
        private bool _processignAuthRequest = false;
        private string _message = "";

        protected override void OnInitialized()
        {
            _formContext = new(_authUserModel);
            _formContext.AddDataAnnotationsValidation();
        }

        private async Task OnValidFormSubmit_LogInAsync()
        {
            _processignAuthRequest = true;

            try
            {
                if (!_authUserModel.IsValidEmail(_authUserModel.Email))
                {
                    _message = "Fejl, den indtastede email er ikke gyldig.";
                    return;
                }

                if (!_formContext.Validate())
                {
                    _message = "Fejl i form, et eller flere felter er ugyldige.";
                    return;
                }

                try
                {
                    var authUser = await UserService.AuthenticateUserLoginAsync(_authUserModel.Email, _authUserModel.Password);

                    if (authUser is not null)
                    {
                        await MyAuthStateProvider.MarkUserAsAuthenticated(authUser);

                        NavigationManager.NavigateTo("/admin", true);
                        return;
                    }
                }
                catch (System.Exception)
                {
                    _message = "Autentificerings Fejl.";
                }

                _message = "Fejl, forkert email eller adgangskode.";
            }
            finally
            {
                _processignAuthRequest = false;
            }
        }
    }
}
