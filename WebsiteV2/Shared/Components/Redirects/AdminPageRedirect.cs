using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Redirects
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
