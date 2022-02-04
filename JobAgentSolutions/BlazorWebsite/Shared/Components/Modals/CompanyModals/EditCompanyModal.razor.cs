using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Extensions;
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

        private readonly bool _showError = false;
        private string _errorMessage = string.Empty;

        private async Task OnValidSubmit_UpdateCompanyAsync()
        {
            if (Model.IsProcessing is true)
            {
                return;
            }
            using (var _ = Model.TimedEndOfOperation())
            {


                Company company = new()
                {
                    Id = Model.CompanyId,
                    Name = Model.Name
                };

                var result = await CompanyService.UpdateAsync(company);

                if (result is null)
                {
                    _errorMessage = "Fejl under opdatering af Virksomheden.";
                    return;
                }
            }

            if (Model.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCompany");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.CompanyId}");
            }
        }
    }
}
