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
        [Inject] protected MessageClearProvider MessageClearProvider { get; set; }

        public string Message { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                RefreshProvider.RefreshRequest += RefreshContent;
                PaginationService.OnPageChange += OnPageChange_RenderPage;
                PaginationService.OnPageSizeChange += OnPageSizeChange_RenderPage;
                MessageClearProvider.MessageCleared += OnMessageCleared_ClearMessage;
            }
        }

        protected override void OnParametersSet()
        {
            PaginationService.CurrentPage = 1;
        }

        public async Task OnPageChange_RenderPage()
        {
            await InvokeAsync(StateHasChanged);
        }

        public async Task OnPageSizeChange_RenderPage()
        {
            await InvokeAsync(StateHasChanged);
        }

        public void OnMessageCleared_ClearMessage(object sender, bool isCleared)
        {
            if (isCleared)
            {
                Message = string.Empty;
            }
        }

        public abstract Task RefreshContent();

        public virtual void Dispose()
        {
            RefreshProvider.RefreshRequest -= RefreshContent;
            PaginationService.OnPageChange -= OnPageChange_RenderPage;
            PaginationService.OnPageSizeChange -= OnPageSizeChange_RenderPage;
            MessageClearProvider.MessageCleared -= OnMessageCleared_ClearMessage;
        }
    }
}
