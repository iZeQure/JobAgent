using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class RemoveJobAdvertModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobAdvert(int id)
        {
            try
            {
                _isProcessing = true;

                JobAdvert jobAdvert = new()
                {
                    Id = id,
                    RegistrationDateTime = DateTime.Now
                };


                var result = await JobAdvertService.RemoveAsync(jobAdvert);

                if (result)
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
            RefreshProvider.CallRefreshRequest();
        }
    }
}
