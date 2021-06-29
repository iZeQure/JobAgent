using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.Contract
{
    public partial class RemoveContractModal
    {
        [Parameter]
        public int ContractId { get; set; }

        [Inject]
        public ContractService ContractService { get; set; }

        [Inject]
        public IRefreshProvider RefreshProvider { get; set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        protected async Task OnClick_RemoveContract(int id)
        {
            await ContractService.RemoveContractAsync(id);

            RefreshProvider.CallRefreshRequest();

            await JS.InvokeVoidAsync("confirmRemove", "RemoveContractModal");
        }
    }
}
