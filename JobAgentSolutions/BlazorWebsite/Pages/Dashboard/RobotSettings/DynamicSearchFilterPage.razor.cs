using BlazorWebsite.Data.FormModels;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.RobotSettings
{
    public partial class DynamicSearchFilterPage
    {
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }

        private DynamicSearchFilterModel _dynamicSearchFilterModel = new();
        private IEnumerable<IDynamicSearchFilter> _dynamicSearchFilters;
        private IEnumerable<ICategory> _categories = new List<Category>();
        private IEnumerable<ISpecialization> _specializations = new List<Specialization>();
        private IDynamicSearchFilter _dynamicSearchFilter;

        private int _dynamicSearchFilterId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var dynamicSearchFilterTask = DynamicSearchFilterService.GetAllAsync();
                var categoryTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                await Task.WhenAll(dynamicSearchFilterTask, categoryTask, specializationTask);

                _dynamicSearchFilters = dynamicSearchFilterTask.Result;
                _categories = categoryTask.Result;
                _specializations = specializationTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _dynamicSearchFilterId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _dynamicSearchFilter = await DynamicSearchFilterService.GetByIdAsync(id);

                _dynamicSearchFilterModel = new DynamicSearchFilterModel
                {
                    Id = _dynamicSearchFilter.Id,
                    CategoryId = _dynamicSearchFilter.CategoryId,
                    Specializationid = _dynamicSearchFilter.SpecializationId,
                    Key = _dynamicSearchFilter.Key
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        public override async Task RefreshContent()
        {
            try
            {
                var filters = await DynamicSearchFilterService.GetAllAsync();

                if (filters == null)
                {
                    return;
                }

                _dynamicSearchFilters = filters;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
