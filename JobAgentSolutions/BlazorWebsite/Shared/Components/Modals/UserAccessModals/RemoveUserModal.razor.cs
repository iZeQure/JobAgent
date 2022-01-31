using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.UserAccessModals
{
    public partial class RemoveUserModal : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IUserService UserService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;
        private string _sessionUserEmail;


        private async Task OnClick_RemoveJobPageAsync(int id)
        {
            try
            {
                _isProcessing = true;

                var session = await AuthState;

                foreach (var sessionClaim in session.User.Claims)
                {
                    if (sessionClaim.Type == ClaimTypes.Email)
                    {
                        _sessionUserEmail = sessionClaim.Value;
                    }
                }

                var user = await UserService.GetUserByIdAsync(id);

                if (user.Email == _sessionUserEmail)
                {
                    _errorMessage = "Du kan ikke slette din egen bruger.";
                    return;
                }

                var result = await UserService.RemoveAsync(user);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne Brugeren, den er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveUser");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne Loggen grundet ukendt fejl.";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void CancelRequest()
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
