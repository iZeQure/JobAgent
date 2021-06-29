using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.Contract
{
    public partial class EditContractModal
    {
        [Parameter]
        public ContractModel ContractModel { get; set; }

        [Inject]
        protected DataService DataService { get; set; }

        [Inject]
        protected ContractService ContractService { get; set; }

        [Inject]
        protected IRefreshProvider RefreshProvider { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected private IEnumerable<Company> Companies { get; set; }
        protected private IEnumerable<User> Users { get; set; }

        private bool IsProcessing { get; set; } = false;
        private bool ShowError { get; set; } = false;

        private string ErrorMessage { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private Task LoadData()
        {
            Task.Run(async () =>
            {
                Companies = await DataService.GetAllCompaniesAsync();
                Users = await DataService.GetUsersAsync();
            });

            return Task.CompletedTask;
        }

        private async Task OnValidSubmit_UpdateContract()
        {
            IsProcessing = true;

            await ContractService.UpdateContractAsync(ContractModel);

            IsProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("modalToggle", "EditContractModal");
            await JSRuntime.InvokeVoidAsync("OnInformationChangeAnimation", $"{ContractModel.Id}");
        }

        private void CancelRequest(MouseEventArgs e)
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
