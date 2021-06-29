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
    public partial class EditContractModal
    {
        [Parameter] public EditContext ContractModelContext { get; set; }
        [Inject] protected IContractService ContractService { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private IEnumerable<Company> _companies = new List<Company>();
        private IEnumerable<IUser> _users = new List<User>();

        private bool _isProcessing = false;
        private bool _isLoadingData = false;
        private bool _showError = false;
        private string _errorMessage = string.Empty;

        protected override Task OnInitializedAsync()
        {
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
                    await TaskExtProvider.WhenAll(companiesTask, usersTask);

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

        private async Task OnValidSubmit_UpdateContract()
        {
            _isProcessing = true;

            if (ContractModelContext.Model is ContractModel model)
            {

                var contract = new Contract(
                new Company(model.SignedWithCompany, 0, "", ""),
                new User(model.SignedByUser, null, null, null, "", "", ""),
                model.ContractFileName, model.RegistrationDate, model.ExpiryDate);

                await ContractService.UpdateAsync(contract, _tokenSource.Token);

                _isProcessing = false;

                RefreshProvider.CallRefreshRequest();

                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditContract");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{model.Id}");
            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            _tokenSource.Cancel();
            RefreshProvider.CallRefreshRequest();
        }
    }
}
