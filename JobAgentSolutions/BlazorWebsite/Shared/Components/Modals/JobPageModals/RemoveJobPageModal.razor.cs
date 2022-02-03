using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.JobPageModals
{
    public partial class RemoveJobPageModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobPageService JobAdvertService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPageAsync(int id)
        {
            try
            {
                _isProcessing = true;

                JobPage jobPage = new()
                {
                    Id = id
                };

                var result = await JobAdvertService.RemoveAsync(jobPage);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne JobSiden, siden er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveJobPage");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne JobSiden grundet ukendt fejl.";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void CancelRequest()
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
