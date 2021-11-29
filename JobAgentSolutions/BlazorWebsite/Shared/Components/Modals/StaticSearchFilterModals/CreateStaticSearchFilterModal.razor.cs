using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
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
        private bool _isProcessing = false;

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
                    foreach (var filterType in _filterTypes)
                    {
                        if (staticFilter.FilterType.Id == filterType.Id)
                        {
                            staticFilter.FilterType.Name = filterType.Name;
                            staticFilter.FilterType.Description = filterType.Description;
                        }
                    }
                }

                _staticSearchFilterModel.FilterType = new();

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
            _isProcessing = true;
            try
            {
                StaticSearchFilter staticSearchFilter = new()
                {
                    Id = _staticSearchFilterModel.Id,
                    Key = _staticSearchFilterModel.Key,
                    FilterType = _staticSearchFilterModel.FilterType

                };

                bool isCreated = false;
                var result = await StaticcSearchFilterService.CreateAsync(staticSearchFilter);

                if (result.Id == _staticSearchFilterModel.Id && result.Key == _staticSearchFilterModel.Key)
                {
                    isCreated = true;
                }

                if (!isCreated)
                {
                    _errorMessage = "Kunne ikke oprette søgeord grundet ukendt fejl";
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateStaticSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_staticSearchFilterModel.Id}");

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
            _staticSearchFilterModel = new();
            _staticSearchFilterModel.FilterType = new();
            _editContext = new(_staticSearchFilterModel);
            StateHasChanged();
        }
    }
}
