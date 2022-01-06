using Microsoft.AspNetCore.Components;

namespace BlazorWebsite.Shared.Components.Redirects
{
    /// <summary>
    /// Handles the redirecting to Home Page.
    /// </summary>
    public class HomePageRedirect : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("", true);
        }
    }
}
