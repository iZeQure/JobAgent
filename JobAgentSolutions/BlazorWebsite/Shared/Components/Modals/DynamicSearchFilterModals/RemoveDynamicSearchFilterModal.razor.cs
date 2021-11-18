using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.DynamicSearchFilterModals
{
    public partial class RemoveDynamicSearchFilterModal
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPage(int id)
        {
            try
            {
                _isProcessing = true;

                DynamicSearchFilter dynamicSearchFilter = new()
                {
                    Id = id
                };

                var result = await DynamicSearchFilterService.RemoveAsync(dynamicSearchFilter);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne Søgeordet, siden er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveDynamicSearchFilter");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne søgeordet grundet ukendt fejl.";
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
