using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.DbLogModals
{
    public partial class RemoveDbLogModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPage(int id)
        {
            try
            {
                _isProcessing = true;

                DbLog dbLog = new()
                {
                    Id = id
                };

                var result = await LogService.RemoveAsync(dbLog);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne Loggen, den er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveDbLog");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne Loggen grundet ukendt fejl.";
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
