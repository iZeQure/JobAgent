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
    public partial class StaticSearchFilterPage
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IStaticSearchFilterService StaticSearchFilterService { get; set; }
        [Inject] protected IFilterTypeService FilterTypeService { get; set; }

        private StaticSearchFilterModel _staticSearchFilterModel = new();
        private IEnumerable<IStaticSearchFilter> _staticSearchFilters = new List<StaticSearchFilter>();
        private IEnumerable<IFilterType> _filterTypes = new List<FilterType>();
        private IStaticSearchFilter _staticSearchFilter;

        private int _staticSearchFilterId = 0;
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
                var staticSearchFilterTask = StaticSearchFilterService.GetAllAsync();
                var filterTypeTask = FilterTypeService.GetAllAsync();

                await Task.WhenAll(staticSearchFilterTask, filterTypeTask);

                _staticSearchFilters = staticSearchFilterTask.Result;
                _filterTypes = filterTypeTask.Result;

                foreach(var staticFilter in _staticSearchFilters)
                {
                    foreach(var filterType in _filterTypes)
                    {
                        if(staticFilter.FilterType.Id == filterType.Id)
                        {
                            staticFilter.FilterType.Name = filterType.Name;
                            staticFilter.FilterType.Description = filterType.Description;
                        }
                    }
                }

            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _staticSearchFilterId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _staticSearchFilter = await StaticSearchFilterService.GetByIdAsync(id);

                _staticSearchFilterModel = new StaticSearchFilterModel
                {
                    Id = _staticSearchFilter.Id,
                    FilterType = _staticSearchFilter.FilterType,
                    Key = _staticSearchFilter.Key
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
                var filters = await StaticSearchFilterService.GetAllAsync();
                var filtertypes = await FilterTypeService.GetAllAsync();

                if (filters == null || filtertypes == null) return;

                _staticSearchFilters = filters;
                _filterTypes = filtertypes;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
