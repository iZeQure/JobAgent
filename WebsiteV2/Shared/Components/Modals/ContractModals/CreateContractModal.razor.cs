using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.Contract
{
    public partial class CreateContractModal
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshService { get; set; }
        [Inject] protected IFileService FileService { get; set; }
        [Inject] protected ContractService ContractService { get; set; }

        private CancellationTokenSource cancellation;
        private ContractModel contractModel;
        private IEnumerable<User> users;
        private IEnumerable<Company> companies;

        long maxFileSize = 1024 * 1024 * 15;

        private bool isProcessing = false;
        private bool isLoadingData = false;

        private string contractDataUrlForPreview = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            cancellation = new();
            contractModel = new();

            users = new List<User>();
            companies = new List<Company>();

            isLoadingData = true;

            companies = await DataService.GetCompaniesWithOutContractAsync();
            users = await DataService.GetUsersAsync();

            isLoadingData = false;
        }

        private async Task OnValidSubmit_CreateContract()
        {
            isProcessing = true;

            var buffer = new byte[contractModel.Contract.Size];

            await contractModel.Contract.OpenReadStream(maxFileSize).ReadAsync(buffer, cancellation.Token);

            string uploadedFileName = await FileService.UploadFileAsync(buffer);

            if (string.IsNullOrEmpty(uploadedFileName))
            {
                isProcessing = false;
                return;
            }

            // Process Contract Request.
            contractModel.ContractFileName = uploadedFileName;
            await ContractService.CreateContractAsync(contractModel);

            isProcessing = false;

            RefreshService.CallRefreshRequest();
            await JSRuntime.InvokeVoidAsync("modalToggle", "CreateContractModal");

            contractModel = new();
            contractDataUrlForPreview = string.Empty;
        }

        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            contractModel.Contract = e.File;
            contractModel.ContractFileName = contractModel.Contract.Name;

            var buffer = new byte[contractModel.Contract.Size];

            await contractModel.Contract.OpenReadStream().ReadAsync(buffer, cancellation.Token);
            contractDataUrlForPreview = $"data:{contractModel.Contract.ContentType};base64,{FileService.EncodeFileToBase64(buffer)}";
        }

        private void RemoveUploadedFile(MouseEventArgs e)
        {
            contractModel.ContractFileName = string.Empty;
            contractDataUrlForPreview = string.Empty;
        }

        private void CancelRequest(MouseEventArgs e)
        {
            contractModel = new ContractModel();
            contractDataUrlForPreview = string.Empty;
        }
    }
}
