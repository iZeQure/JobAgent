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
        [Parameter] public EditContext CompanyModelContext { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CancellationTokenSource tokenSource = new();
        private bool _isProcessing = false;
        private bool _showError = false;
        private string _errorMessage = string.Empty;

        private async Task OnValidSubmit_UpdateCompany()
        {
            _isProcessing = true;

            if (CompanyModelContext.Model is CompanyModel model)
            {
                Company company = new(
                    model.CompanyId,
                    model.CVR,
                    model.Name,
                    model.ContactPerson);

                await CompanyService.UpdateAsync(company, tokenSource.Token);

                _isProcessing = false;

                RefreshProvider.CallRefreshRequest();

                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCompany");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{model.CompanyId}");
            }
        }
    }
}
