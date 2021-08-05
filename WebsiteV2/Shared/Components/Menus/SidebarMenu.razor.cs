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
        

        private const string NAVLINK_JOB_PREFIX = "/job/";
        private readonly CancellationTokenSource _tokenSource = new();

        private IEnumerable<Category> _menu = new List<Category>();
        private string _searchForEducation = "";
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

        private async Task OnInputChange_ApplySearchFilter(ChangeEventArgs e)
        {
            string getValue = e.Value.ToString().ToLower();

            if (string.IsNullOrEmpty(getValue))
            {
                await UpdateContentAsync();
                return;
            }

            _menu = (await MenuService.InitializeMenu(_tokenSource.Token)).Where(x => x.Name.ToLower().Contains(getValue));
            StateHasChanged();
        }

        private async Task OnClick_SearchForEducation()
        {
            if (string.IsNullOrEmpty(_searchForEducation))
            {
                await UpdateContentAsync();
                return;
            }

            _menu = (await MenuService.InitializeMenu(_tokenSource.Token)).Where(x => x.Name.ToLower().Contains(_searchForEducation));
            StateHasChanged();
        }

        private static string GetAccordionMenuPanelCssId(int number)
        {
            return $"menuAccordionPanel-collapse{number}";
        }

        private static string GenerateNavLinkLocation(int categoryId = 0, int specializationId = 0, string linkLocation = "")
        {
            switch (linkLocation)
            {
                case "CategoryJob":
                    return NAVLINK_JOB_PREFIX + $"{categoryId}";
                case "SpecializationJob":
                    return NAVLINK_JOB_PREFIX + $"{categoryId}/{specializationId}";
                default:
                    return NAVLINK_JOB_PREFIX + "uncategorized";
            }
        }
    }
}
