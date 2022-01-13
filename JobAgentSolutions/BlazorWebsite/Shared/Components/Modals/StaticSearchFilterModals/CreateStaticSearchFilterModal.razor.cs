using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.StaticSearchFilterModals
{
    public partial class CreateStaticSearchFilterModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IStaticSearchFilterService StaticcSearchFilterService { get; set; }
        [Inject] protected IFilterTypeService FilterTypeService { get; set; }

        private StaticSearchFilterModel _staticSearchFilterModel = new();
        private IEnumerable<IStaticSearchFilter> _staticSearchFilters;
        private IEnumerable<IFilterType> _filterTypes;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_staticSearchFilterModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformation();
        }

        private async Task LoadModalInformation()
        {
            _isLoading = true;

            try
            {
                var companyTask = StaticcSearchFilterService.GetAllAsync();
                var filterTypeTask = FilterTypeService.GetAllAsync();

                try
                {
                    await TaskExtProvider.WhenAll(companyTask, filterTypeTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _staticSearchFilters = companyTask.Result;
                _filterTypes = filterTypeTask.Result;

                foreach (var staticFilter in _staticSearchFilters)
                {
                    var filterType = _filterTypes.FirstOrDefault(x => x.Id == staticFilter.FilterType.Id);
                    staticFilter.FilterType.Name = filterType.Name;
                    staticFilter.FilterType.Description = filterType.Description;
                }
            }
            catch (Exception)
            {
                _errorMessage = "Uventet fejl. Prøv at genindlæse siden.";
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit_CreateJobAdvertAsync()
        {
            if (_staticSearchFilterModel.IsProcessing is true)
            {
                return;
            }

            IStaticSearchFilter result = null;

            using (var _ = _staticSearchFilterModel.TimedEndOfOperation())
            {
                StaticSearchFilter staticSearchFilter = new()
                {
                    Id = _staticSearchFilterModel.Id,
                    Key = _staticSearchFilterModel.Key,
                    FilterType = _staticSearchFilterModel.FilterType

                };

                result = await StaticcSearchFilterService.CreateAsync(staticSearchFilter);

                if (result is null)
                {
                    _errorMessage = "Fejl under oprettelse af filter.";
                    return;
                }
            }

            if (_staticSearchFilterModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateStaticSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{result.Id}");
            }
        }

        private void OnClick_CancelRequest()
        {
            _staticSearchFilterModel = new();
            _staticSearchFilterModel.FilterType = new();
            _editContext = new(_staticSearchFilterModel);
            StateHasChanged();
        }
    }
}
