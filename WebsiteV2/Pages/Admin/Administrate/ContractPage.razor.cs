using ObjectLibrary.Common;
using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class ContractPage : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IContractService ContractService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private EditContext _contractModelEditContext;
        private ContractModel _contractModel = new();
        private ShowContractModel _showContractModel = new();
        private IEnumerable<Contract> _contracts = new List<Contract>();
        private IEnumerable<Company> _companies = new List<Company>();

        private int _contractId = 0;
        private string _errorMessage = string.Empty;
        private bool _isDisabled = false;
        private bool _isLoadingData = false;

        protected override Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshContent;
            _contractModelEditContext = new(_contractModel);

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isLoadingData = true;

                _contracts = await ContractService.GetAllAsync(_tokenSource.Token);
                _companies = await CompanyService.GetAllAsync(_tokenSource.Token);

                if (!_companies.Any()) _isDisabled = true;
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RefreshContent()
        {
            _showContractModel = new();

            try
            {
                var contracts = await ContractService.GetAllAsync(_tokenSource.Token);

                if (contracts != null)
                {
                    _contracts = contracts;
                    return;
                }

                _contracts = null;

                _errorMessage = "Kunne ikke indlæse kontrakter.";
            }
            catch (Exception) { _errorMessage = "Ukendt Fejl ved opdatering af kontrakter."; }
            finally { StateHasChanged(); }
        }

        private void OnClick_RemoveContractModal(int id)
        {
            _contractId = id;
        }

        private async Task OnClick_ShowContractModal(string name, string companyName)
        {
            _showContractModel.ContractIsLoading = true;

            if (true == string.IsNullOrEmpty(name))
            {
                _showContractModel.ViewContractMessage = "Kontraktens navn, blev ikke indlæst korrekt, prøv igen.";
                return;
            }

            try
            {
                var contractIsExisting = ContractService.CheckFileExists(name);

                if (false == contractIsExisting)
                {
                    _showContractModel.ViewContractMessage = "Kontrakten findes ikke i systemet.";
                    return;
                }

                var fileInformation = await ContractService.GetFileFromDirectoryAsync(name, _tokenSource.Token);
                var encodedFileInformation = ContractService.EncodeFileToBase64(fileInformation);

                if (true == string.IsNullOrEmpty(encodedFileInformation))
                {
                    _showContractModel.ViewContractMessage = "Kontraktens informationer blev ikke indlæst korrekt.";
                    return;
                }

                _showContractModel = new()
                {
                    ContractExists = true,
                    ContractIsLoading = false,
                    ContractForCompanyName = companyName,
                    EncodedContractData = encodedFileInformation,
                    ContractName = name,
                    ViewContractMessage = string.Empty
                };
            }
            catch (UnauthorizedAccessException)
            {
                _showContractModel.ViewContractMessage = "Den givne konto, har ikke rettigheder til at se dette.";
            }
            catch (Exception)
            {
                _showContractModel.ViewContractMessage = $"Ukendt fejl.";
            }
            finally
            {
                _showContractModel.ContractIsLoading = false;
            }
        }

        private async Task OnClick_OpenEditModal(int contractId)
        {
            var contract = await ContractService.GetByIdAsync(contractId, _tokenSource.Token);

            _contractModel = new ContractModel()
            {
                Id = contract.Id,
                SignedWithCompany = contract.company.Id,
                SignedByUser = contract.user.Id,
                ContactPerson = contract.company.ContactPerson,
                ContractFileName = contract.name,
                RegistrationDate = contract.registrationDateTime,
                ExpiryDate = contract.expiryDateTime
            };

            _contractModelEditContext = new(_contractModel);
            _contractModelEditContext.AddDataAnnotationsValidation();
        }
    }
}
