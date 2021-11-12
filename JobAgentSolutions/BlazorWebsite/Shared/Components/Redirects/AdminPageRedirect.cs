using Microsoft.AspNetCore.Components;

namespace BlazorWebsite.Shared.Components.Redirects
{
    /// <summary>
    /// Handles the redirection to Admin Page.
    /// </summary>
    public class AdminPageRedirect : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/admin", true);
        }
    }
}
