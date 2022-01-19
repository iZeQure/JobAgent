using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
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

namespace BlazorWebsite.Shared.Components.Modals.DynamicSearchFilterModals
{
    public partial class CreateDynamicSearchFilterModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private DynamicSearchFilterModel _dynamicSearchFilterModel = new();
        private IEnumerable<ICategory> _categories;
        private IEnumerable<ISpecialization> _specializations;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;
        private bool _hasSpecs = false;

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
                var categoryTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                try
                {
                    await TaskExtProvider.WhenAll(categoryTask, specializationTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

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
            if (_dynamicSearchFilterModel.IsProcessing is true)
            {
                return;
            }

            IDynamicSearchFilter result = null;

            using (var _ = _dynamicSearchFilterModel.TimedEndOfOperation())
            {
                DynamicSearchFilter dynamicSearchFilter = new()
                {
                    CategoryId = _dynamicSearchFilterModel.CategoryId,
                    SpecializationId = _dynamicSearchFilterModel.Specializationid,
                    Key = _dynamicSearchFilterModel.Key
                };

                result = await DynamicSearchFilterService.CreateAsync(dynamicSearchFilter);

                if (result is null)
                {
                    _errorMessage = "Fejl under oprettelse af filteret.";
                    return;
                }
            }

            if (_dynamicSearchFilterModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateDynamicSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{result.Id}"); 
            }
        }

        private async Task OnChange_CheckCategorySpecializations(ChangeEventArgs e)
        {
            _dynamicSearchFilterModel.CategoryId = int.Parse(e.Value.ToString());
            ICategory category = await CategoryService.GetCategoryWithSpecializationsById(_dynamicSearchFilterModel.CategoryId);

            if (category.Specializations.Any())
            {
                _hasSpecs = true;
            }
            else
            {
                _hasSpecs = false;
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
