using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class CreateCompanyModal
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private EditContext _editContext;
        private CompanyModel _companyModel = new();
        private bool _isProcessing = false;
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
            if (_companyModel != null)
            {
                _isProcessing = true;

                Company company = new(
                    _companyModel.CompanyId,
                    _companyModel.CVR,
                    _companyModel.Name,
                    _companyModel.ContactPerson);

                var result = await CompanyService.CreateAsync(company, _tokenSource.Token);
                _isProcessing = false;

                if (result == 1)
                {
                    _companyModel = new CompanyModel();
                    RefreshProvider.CallRefreshRequest();
                    await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCompany");
                    await Task.CompletedTask;
                }                
            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            _tokenSource.Cancel();
            _companyModel = new CompanyModel();
        }
    }
}
