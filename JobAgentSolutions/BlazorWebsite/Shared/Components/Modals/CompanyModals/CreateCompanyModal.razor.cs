using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class CreateCompanyModal : ComponentBase
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private EditContext _editContext;
        private CompanyModel _companyModel = new();
        private bool _showError = false;
        private string _errorMessage = string.Empty;

        protected override Task OnInitializedAsync()
        {
            _editContext = new(_companyModel);
            _editContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
        }

        private async Task OnValidSubmit_CreateCompany()
        {
            if (_companyModel.IsProcessing is true)
            {
                return;
            }
            using (var _ = _companyModel.TimedEndOfOperation())
            {
                Company company = new()
                {
                    Id = _companyModel.CompanyId,
                    Name = _companyModel.Name
                };

                var result = await CompanyService.CreateAsync(company);

                if (result is null)
                {
                    _errorMessage = "Fejl under oprettelse af Virksomehed.";
                    return;
                }
            }

            if (_companyModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCompany");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_companyModel.CompanyId}");
            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            _companyModel = new();
            StateHasChanged();
        }
    }
}
