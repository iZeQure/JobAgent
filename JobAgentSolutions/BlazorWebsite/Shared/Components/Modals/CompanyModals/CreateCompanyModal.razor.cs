using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class CreateCompanyModal
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

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
            try
            {
                if (_companyModel != null)
                {
                    _isProcessing = true;

                    Company company = new()
                    {
                        Id = _companyModel.CompanyId,
                        Name = _companyModel.Name
                    };

                    bool isCreated = false;
                    var result = await CompanyService.CreateAsync(company);

                    if (result.Id == _companyModel.CompanyId && result.Name == _companyModel.Name)
                    {
                        isCreated = true;
                    }

                    if (!isCreated)
                    {
                        _errorMessage = "Kunne ikke oprette virksomheden grundet ukendt fejl.";
                    }

                    RefreshProvider.CallRefreshRequest();
                    await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCompany");
                    await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_companyModel.CompanyId}");

                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isProcessing = false;

            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            _isProcessing = false;
            _companyModel = new();
            StateHasChanged();
        }
    }
}
