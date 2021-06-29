using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals
{
    public partial class EditCompanyModal
    {
        [Parameter] public CompanyModel CompanyModel { get; set; }
        [Inject] protected CompanyService CompanyService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CancellationTokenSource tokenSource = new();
        private bool IsProcessing { get; set; } = false;
        private bool ShowError { get; set; } = false;
        private string ErrorMessage { get; set; } = string.Empty;

        private async Task OnValidSubmit_UpdateCompany()
        {
            IsProcessing = true;

            Company company = new(
                    CompanyModel.CompanyId,
                    CompanyModel.CVR,
                    CompanyModel.Name,
                    CompanyModel.ContactPerson);

            await CompanyService.UpdateAsync(company, tokenSource.Token);

            IsProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("modalToggle", "EditCompanyModal");
            await JSRuntime.InvokeVoidAsync("OnInformationChangeAnimation", $"{CompanyModel.CompanyId}");
        }
    }
}
