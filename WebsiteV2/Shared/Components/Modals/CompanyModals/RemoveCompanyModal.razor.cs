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

namespace BlazorServerWebsite.Shared.Components.Modals
{
    public partial class RemoveCompanyModal
    {
        [Parameter] public int CompanyId { get; set; }
        [Inject] protected RefreshProvider RefreshProvider { get; set; }
        [Inject] protected private CompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CancellationTokenSource tokenSource = new();
        protected private CompanyModel CompanyModel { get; set; }
        private bool IsProcessing { get; set; }

        private async Task OnClick_RemoveCompany(int id)
        {
            IsProcessing = true;

            Company company = new(
                   CompanyModel.CompanyId,
                   CompanyModel.CVR,
                   CompanyModel.Name,
                   CompanyModel.ContactPerson);

            await CompanyService.DeleteAsync(company, tokenSource.Token);

            IsProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("modalToggle", "RemoveCompanyModal");
        }

        private void CancelRequest(MouseEventArgs e)
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
