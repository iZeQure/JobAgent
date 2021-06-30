using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using SecurityLibrary.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.ContractModals
{
    public partial class CreateContractModal : ComponentBase
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshService { get; set; }
        [Inject] protected IContractService ContractService { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private readonly long _maxFileSize = 1024 * 1024 * 15;
        private IEnumerable<IUser> _users = new List<User>();
        private IEnumerable<Company> _companies = new List<Company>();
        private EditContext _editContext;
        private ContractModel _model = new();

        private bool _isProcessing = false;
        private bool _isLoadingData = false;

        private string _contractDataUrlForPreview = string.Empty;

        protected override Task OnInitializedAsync()
        {
            _editContext = new(_model);
            _editContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isLoadingData = true;
                // Get tasks to load.
                var companiesTask = CompanyService.GetAllAsync(_tokenSource.Token);
                var usersTask = UserService.GetAllAsync(_tokenSource.Token);

                // Wait for data to be loaded.
                try
                {
                    await Task.WhenAll(companiesTask, usersTask);

                    _companies = companiesTask.Result;
                    _users = usersTask.Result;

                    _isLoadingData = false;
                }
                catch (Exception)
                {
                    _isLoadingData = false;
                    throw;
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task OnValidSubmit_CreateContract()
        {
            _isProcessing = true;

            var buffer = new byte[_model.ContractFile.Size];

            await _model.ContractFile.OpenReadStream(_maxFileSize).ReadAsync(buffer, _tokenSource.Token);

            string uploadedFileName = await ContractService.UploadFIleAsync(buffer, _tokenSource.Token);

            if (string.IsNullOrEmpty(uploadedFileName))
            {
                _isProcessing = false;
                return;
            }

            // Process Contract Request.
            _model.ContractFileName = uploadedFileName;
            var contract = new Contract(
                new Company(_model.SignedWithCompany, 0, "", ""),
                new User(_model.SignedByUser, null, null, null, "", "", ""),
                _model.ContractFileName, _model.RegistrationDate, _model.ExpiryDate);

            await ContractService.CreateAsync(contract, _tokenSource.Token);

            _isProcessing = false;

            RefreshService.CallRefreshRequest();
            await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateContract");

            _model = new();
            _contractDataUrlForPreview = string.Empty;
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            _model.ContractFile = e.File;
            _model.ContractFileName = _model.ContractFile.Name;

            var buffer = new byte[_model.ContractFile.Size];

            await _model.ContractFile.OpenReadStream().ReadAsync(buffer, _tokenSource.Token);
            _contractDataUrlForPreview = $"data:{_model.ContractFile.ContentType};base64,{ContractService.EncodeFileToBase64(buffer)}";
        }

        private void RemoveUploadedFile(MouseEventArgs e)
        {
            _model.ContractFileName = string.Empty;
            _contractDataUrlForPreview = string.Empty;
        }

        private void CancelRequest(MouseEventArgs e)
        {
            _tokenSource.Cancel();
            _model = new ContractModel();
            _contractDataUrlForPreview = string.Empty;
        }
    }
}
