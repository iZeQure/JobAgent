using BlazorServerWebsite.Data.FormModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals
{
    public partial class CreateCompanyModal
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected CompanyService CompanyService { get; set; }

        protected private CompanyModel CompanyModel { get; set; } = new CompanyModel();

        private bool IsProcessing { get; set; } = false;
        private bool ShowError { get; set; } = false;

        private string ErrorMessage { get; set; } = string.Empty;

        private async void OnValidSubmit_CreateCompany()
        {
            IsProcessing = true;

            await CompanyService.CreateCompanyAsync(CompanyModel);

            CompanyModel = new CompanyModel();

            IsProcessing = false;

            await JSRuntime.InvokeVoidAsync("modalToggle", "CreateCompanyModal");
        }

        private void CancelRequest(MouseEventArgs e)
        {
            CompanyModel = new CompanyModel();
        }
    }
}
