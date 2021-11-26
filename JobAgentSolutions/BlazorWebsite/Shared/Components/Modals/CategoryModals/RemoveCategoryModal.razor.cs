using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CategoryModals
{
    public partial class RemoveCategoryModal : ComponentBase
    {
        [Parameter] public int Id { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveJobPage(int id)
        {
            try
            {
                _isProcessing = true;

                Category category = new()
                {
                    Id = id
                };

                var result = await CategoryService.RemoveAsync(category);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne uddannelsen, den er muligvis allerede slettet.";

                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveCategory");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne uddannelsen grundet ukendt fejl.";
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
