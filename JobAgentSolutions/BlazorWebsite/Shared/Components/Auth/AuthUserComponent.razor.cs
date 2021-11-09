using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Common.Users.Factory;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
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
        private IUser _user;
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
                if (!_authUserModel.IsValidEmail())
                {
                    _message = "Fejl, den indtastede email er ikke gyldig.";
                    return;
                }

                if (!_formContext.Validate())
                {
                    _message = "Fejl i form, et eller flere felter er ugyldige.";
                    return;
                }

                var isAuthValid = await UserService.AuthenticateUserLoginAsync(_authUserModel.Email, _authUserModel.Password);

                if (isAuthValid)
                {
                    _user = await UserService.GetByEmailAsync(_authUserModel.Email);

                    await MyAuthStateProvider.MarkuserAsAuthenticatedAsync(_user);

                    NavigationManager.NavigateTo("/", true);
                    return;
                }

                _message = "Fejl, forkert email eller adgangskode.";
            }
            finally
            {
                _processignAuthRequest = false;
            }
        }
    }

    public class AuthUserModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(255, ErrorMessage = "Email er for long (255 karakter begrænse).")]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsValidEmail()
        {
            try
            {
                var addr = new MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
