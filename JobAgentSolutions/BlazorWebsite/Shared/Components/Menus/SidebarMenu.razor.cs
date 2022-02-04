using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Menus
{
    public partial class SidebarMenu : ComponentBase, IDisposable
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }


        private const string NAVLINK_JOB_PREFIX = "/job/";

        private IEnumerable<ICategory> _menu;
        private string _errorMessage = string.Empty;
        private bool _isLoadingData;

        protected async override Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += UpdateContentAsync;

            try
            {
                _isLoadingData = true;
                _menu = await CategoryService.GetMenuAsync();
            }
            catch (Exception)
            {
                _errorMessage = "Fejl ved indløsning af menue.";
            }
            finally
            {
                _isLoadingData = false;
                StateHasChanged();
            }
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                _isLoadingData = true;
                _menu = await CategoryService.GetMenuAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load Job Categories and Specializations : {ex.Message}");
                _errorMessage = "Ukendt Fejl ved opdatering af kategorier.";
            }
            finally
            {
                _isLoadingData = false;
                StateHasChanged();
            }
        }

        private async Task OnInputChange_ApplySearchFilter(ChangeEventArgs e)
        {
            string getValue = e.Value.ToString().ToLower();

            if (string.IsNullOrEmpty(getValue))
            {
                await UpdateContentAsync();
                return;
            }

            _menu = (await CategoryService.GetMenuAsync())
                .Where(x => x.Name.ToLower().Contains(getValue) || x.Specializations.Any(y => y.Name.ToLower().Contains(getValue)));
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            RefreshProvider.RefreshRequest -= UpdateContentAsync;
        }
    }
}
