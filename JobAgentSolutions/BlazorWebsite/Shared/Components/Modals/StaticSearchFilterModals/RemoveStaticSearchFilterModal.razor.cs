using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.StaticSearchFilterModals
{
    public partial class RemoveStaticSearchFilterModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IStaticSearchFilterService StaticSearchFilterService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPageAsync(int id)
        {
            try
            {
                _isProcessing = true;

                StaticSearchFilter staticSearchFilter = new()
                {
                    Id = id
                };

                var result = await StaticSearchFilterService.RemoveAsync(staticSearchFilter);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne Søgefilteret, siden er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveStaticSearchFilter");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne søgefilteret grundet ukendt fejl.";
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
