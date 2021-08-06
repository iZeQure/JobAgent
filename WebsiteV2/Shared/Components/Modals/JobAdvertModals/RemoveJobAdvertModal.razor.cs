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

namespace BlazorServerWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class RemoveJobAdvertModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }

        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private CancellationTokenSource _tokenSource = new();

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobAdvert(int id)
        {
            try
            {
                _isProcessing = true;

                JobAdvert jobAdvert = new(
                    new VacantJob(id, null, ""),
                    null,
                    null,
                    "",
                    "",
                    DateTime.Now);

                var result = await JobAdvertService.DeleteAsync(jobAdvert, _tokenSource.Token);

                if (result == 0)
                {
                    _errorMessage = "Kunne ikke fjerne stillingsopslag, stillingen er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveJobAdvert");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne stillingsopslag grundet ukendt fejl.";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void CancelRequest()
        {
            _tokenSource.Cancel();

            RefreshProvider.CallRefreshRequest();
        }
    }
}
