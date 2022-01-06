using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.DynamicSearchFilterModals
{
    public partial class CreateDynamicSearchFilterModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private DynamicSearchFilterModel _dynamicSearchFilterModel = new();
        private IEnumerable<IDynamicSearchFilter> _dynamicSearchFilters;
        private IEnumerable<ICategory> _categories;
        private IEnumerable<ISpecialization> _specializations;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;
        private bool _isProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_dynamicSearchFilterModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformation();
        }

        private async Task LoadModalInformation()
        {
            _isLoading = true;

            try
            {
                var companyTask = DynamicSearchFilterService.GetAllAsync();
                var categoryTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                try
                {
                    await TaskExtProvider.WhenAll(companyTask, categoryTask, specializationTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _dynamicSearchFilters = companyTask.Result;
                _categories = categoryTask.Result;
                _specializations = specializationTask.Result;
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
                DynamicSearchFilter dynamicSearchFilter = new()
                {
                    Id = _dynamicSearchFilterModel.Id,
                    CategoryId = _dynamicSearchFilterModel.CategoryId,
                    SpecializationId = _dynamicSearchFilterModel.Specializationid,
                    Key = _dynamicSearchFilterModel.Key
                };

                bool isCreated = false;
                var result = await DynamicSearchFilterService.CreateAsync(dynamicSearchFilter);

                if (result.Id == _dynamicSearchFilterModel.Id && result.Key == _dynamicSearchFilterModel.Key)
                {
                    isCreated = true;
                }

                if (!isCreated)
                {
                    _errorMessage = "Kunne ikke oprette søgeord grundet ukendt fejl";
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateDynamicSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_dynamicSearchFilterModel.Id}");

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
            _dynamicSearchFilterModel = new();
            _editContext = new(_dynamicSearchFilterModel);
            StateHasChanged();
        }
    }
}
