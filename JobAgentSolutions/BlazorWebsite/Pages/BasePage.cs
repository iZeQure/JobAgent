using BlazorWebsite.Data.Providers;
using BlazorWebsite.Data.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages
{
    /// <summary>
    /// Implements base events for pages
    /// </summary>
    public abstract class BasePage : ComponentBase, IDisposable
    {
        [Inject] public IRefreshProvider RefreshProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public PaginationService PaginationService { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                RefreshProvider.RefreshRequest += RefreshContent;
            }
        }

        public abstract Task RefreshContent();

        public void Dispose()
        {
            RefreshProvider.RefreshRequest -= RefreshContent;
        }
    }
}
