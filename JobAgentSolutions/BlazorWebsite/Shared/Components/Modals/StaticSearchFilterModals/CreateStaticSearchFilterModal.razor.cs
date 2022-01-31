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
        [Inject] protected IStaticSearchFilterService StaticSearchFilterService { get; set; }
        [Inject] protected IFilterTypeService FilterTypeService { get; set; }

        private StaticSearchFilterModel _staticSearchFilterModel = new();
        private IEnumerable<IFilterType> _filterTypes;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_staticSearchFilterModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                _filterTypes = await FilterTypeService.GetAllAsync();
                _staticSearchFilterModel.FilterType = (FilterType)_filterTypes.FirstOrDefault();
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

        private async Task OnValidSubmit_CreateStaticSearchFilterAsync()
        {
            try
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

                    result = await StaticSearchFilterService.CreateAsync(staticSearchFilter);

                    if (result is null)
                    {
                        _errorMessage = "Fejl under oprettelse af filter.";
                        return;
                    }
                }
                _staticSearchFilterModel.Id = result.Id;
            }
            catch (Exception)
            {
                _errorMessage = "Noget gik galt.";
                return;
            }

            if (_staticSearchFilterModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateStaticSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_staticSearchFilterModel.Id}");
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
