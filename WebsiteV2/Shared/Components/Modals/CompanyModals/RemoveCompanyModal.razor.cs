using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class RemoveCompanyModal : ComponentBase
    {
        [Parameter] public int CompanyId { get; set; }
        [Inject] protected RefreshProvider RefreshProvider { get; set; }
        [Inject] protected private CompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private bool _isProcessing = false;

        private async Task OnClick_RemoveCompany(int id)
        {
            _isProcessing = true;

            Company company = new(
                   CompanyId,
                   0,
                   "",
                   "");

            await CompanyService.DeleteAsync(company, _tokenSource.Token);

            _isProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveCompany");
        }

        private void CancelRequest(MouseEventArgs e)
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
