using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Menus
{
    public partial class SidebarMenu : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IMenuService MenuService { get; set; }
        

        private readonly CancellationTokenSource _tokenSource = new();

        private IEnumerable<Category> _menu = new List<Category>();
        private Task<int> _countByUncategorized = null;
        private int _categoryId = 0;
        private string _errorMessage = string.Empty;
        private bool _isDisabled = false;
        private bool _isLoadingData = false;

        protected async override Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += UpdateContentAsync;

            await UpdateContentAsync();
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                _isLoadingData = true;

                _menu = await MenuService.InitializeMenu(_tokenSource.Token);

                _isLoadingData = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Job Categories and Specializations : {ex.Message}");
                _errorMessage = "Ukendt Fejl ved opdatering af kategorier.";
            }
            finally { StateHasChanged(); }
        }

        private string GetAccordionMenuPanel(int number)
        {
            return $"menuAccordionPanel-collapse{number}";
        }

    }
}
