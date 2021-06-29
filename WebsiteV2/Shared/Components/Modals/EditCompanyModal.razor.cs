using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals
{
    public partial class EditCompanyModal
    {
        [Parameter]
        public CompanyModel CompanyModel { get; set; }

        [Inject]
        protected RefreshProvider RefreshProvider { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        private bool IsProcessing { get; set; } = false;
        private bool ShowError { get; set; } = false;

        private string ErrorMessage { get; set; } = string.Empty;

        private async Task OnValidSubmit_UpdateCompany()
        {
            IsProcessing = true;

            await CompanyService.UpdateCompanyAsync(CompanyModel);

            IsProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("modalToggle", "EditCompanyModal");
            await JSRuntime.InvokeVoidAsync("OnInformationChangeAnimation", $"{CompanyModel.Id}");
        }
    }
}
