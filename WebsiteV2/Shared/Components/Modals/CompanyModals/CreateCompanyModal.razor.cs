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
    public partial class CreateCompanyModal
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected CompanyService CompanyService { get; set; }

        protected private CompanyModel CompanyModel = new CompanyModel();

        private CancellationTokenSource tokensource = new();
        private bool IsProcessing { get; set; } = false;
        private bool ShowError { get; set; } = false;

        private string ErrorMessage { get; set; } = string.Empty;

        private async Task OnValidSubmit_CreateCompany()
        {
            if (CompanyModel != null)
            {
                IsProcessing = true;

                Company company = new(
                    CompanyModel.CompanyId,
                    CompanyModel.CVR,
                    CompanyModel.Name,
                    CompanyModel.ContactPerson);

                var result = await CompanyService.CreateAsync(company, tokensource.Token);

                if (result == 1)
                {
                    CompanyModel = new CompanyModel();

                    IsProcessing = false;

                    RefreshProvider.CallRefreshRequest();

                    await JSRuntime.InvokeVoidAsync("modalToggle", "CreateCompanyModal");

                    return;
                }
                if(result == 0)
                {
                    RefreshProvider.CallRefreshRequest();
                }

                
            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            CompanyModel = new CompanyModel();
        }
    }
}
