using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class EditCompanyModal : ComponentBase
    {
        [Parameter] public CompanyModel Model { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private bool _isProcessing = false;
        private bool _showError = false;
        private string _errorMessage = string.Empty;

        private async Task OnValidSubmit_UpdateCompany()
        {
            try
            {
                _isProcessing = true;

                Company company = new()
                {
                    Id = Model.CompanyId,
                    Name = Model.Name
                };

                bool isUpdated = false;
                var result = await CompanyService.UpdateAsync(company);

                if (result.Id == company.Id && result.Name == company.Name)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere virksomhed grundet ukendt fejl.";
                }
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCompany");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.CompanyId}");

            }
            finally
            {
                _isProcessing = false;
                StateHasChanged();
            }
        }
    }
}
