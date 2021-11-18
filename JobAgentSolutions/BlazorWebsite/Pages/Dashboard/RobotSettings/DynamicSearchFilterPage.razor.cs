using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
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
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }

        private DynamicSearchFilterModel _dynamicSearchFilterModel = new();
        private IEnumerable<IDynamicSearchFilter> _dynamicSearchFilters;
        private IDynamicSearchFilter _dynamicSearchFilter;

        private int _dynamicSearchFilterId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshContent;

            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var dynamicSearchFilterTask = DynamicSearchFilterService.GetAllAsync();

                await Task.WhenAll(dynamicSearchFilterTask);

                _dynamicSearchFilters = dynamicSearchFilterTask.Result;
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

        private async Task RefreshContent()
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
