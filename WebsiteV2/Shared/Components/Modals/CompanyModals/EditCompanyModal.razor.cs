using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class EditCompanyModal : ComponentBase
    {
        [Parameter] public CompanyModel CompanyModel { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CancellationTokenSource tokenSource = new();
        private bool _isProcessing = false;
        private bool _showError = false;
        private string _errorMessage = string.Empty;

        private async Task OnValidSubmit_UpdateCompany()
        {
            try
            {
                _isProcessing = true;

                Company company = new(
                    CompanyModel.CompanyId,
                    CompanyModel.CVR,
                    CompanyModel.Name,
                    CompanyModel.ContactPerson);

                await CompanyService.UpdateAsync(company, tokenSource.Token);

                RefreshProvider.CallRefreshRequest();

                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCompany");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{CompanyModel.CompanyId}");

            }
            finally
            {
                _isProcessing = false;
                StateHasChanged();
            }
        }
    }
}
