using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.VacantJobModals
{
    public partial class RemoveVacantJobModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPage(int id)
        {
            try
            {
                _isProcessing = true;

                VacantJob vacantJob = new()
                {
                    Id = id
                };

                var result = await VacantJobService.RemoveAsync(vacantJob);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne stillingsopslaget, siden er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveVacantJob");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne stillingsopslaget grundet ukendt fejl.";
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
