using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.StaticSearchFilterModals
{
    public partial class EditStaticSearchFilterModal : ComponentBase
    {
        [Parameter] public StaticSearchFilterModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IStaticSearchFilterService StaticSearchFilterService { get; set; }
        [Inject] protected IFilterTypeService FilterTypeService { get; set; }

        private IEnumerable<IFilterType> _filterTypes = new List<FilterType>();
        private FilterType _filterType = new();
        private int _filterTypeId = 0;

        private string _errorMessage = "";
        private bool _isProcessing = false;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var filterTypeTask = FilterTypeService.GetAllAsync();

                await Task.WhenAll(filterTypeTask);

                _filterTypes = filterTypeTask.Result;

                if (Model.FilterType != null)
                {
                    _filterType = Model.FilterType;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit_EditJobVacancy()
        {
            _isProcessing = true;

            try
            {
                foreach(var item in _filterTypes)
                {
                    if(_filterTypeId == item.Id)
                    {
                        _filterType = new()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Description = item.Description
                        };
                    }
                }

                StaticSearchFilter staticSearchFilter = new()
                {
                    Id = Model.Id,
                    Key = Model.Key,
                    FilterType = _filterType
                };

                bool isUpdated = false;
                var result = await StaticSearchFilterService.UpdateAsync(staticSearchFilter);

                if (result.Id == Model.Id && result.Key == Model.Key)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere SøgeFilteret, grundet ukendt fejl.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditStaticSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new StaticSearchFilterModel();
            Model.FilterType = new FilterType();
            _filterType = new();
            StateHasChanged();
        }
    }
}
