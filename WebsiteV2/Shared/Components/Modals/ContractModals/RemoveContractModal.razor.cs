using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.ContractModals
{
    public partial class RemoveContractModal
    {
        [Parameter] public int ContractId { get; set; }
        [Inject] protected IContractService ContractService { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JS { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private string _exceptionMessage = string.Empty;

        protected async Task OnClick_RemoveContract(int id)
        {
            try
            {
                var contract = new Contract(
                new Company(id, 0, "", ""),
                null, "", DateTime.Now, DateTime.Now);

                var deleteTask = ContractService.DeleteAsync(contract, _tokenSource.Token);

                await Task.WhenAll(deleteTask);

                int result = deleteTask.Result;

                if (result == 1)
                {
                    RefreshProvider.CallRefreshRequest();

                    await JS.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveContract");

                    await Task.CompletedTask;
                }

                _exceptionMessage = "Noget gik galt med at slette kontrakten; prøv igen senere.";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
