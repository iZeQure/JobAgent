using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.DynamicSearchFilterModals
{
    public partial class EditDynamicSearchFilterModal : ComponentBase
    {
        [Parameter] public DynamicSearchFilterModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IDynamicSearchFilterService DynamicSearchFilterService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private IEnumerable<ICategory> _categories = new List<Category>();
        private IEnumerable<ISpecialization> _specializations = new List<Specialization>();

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
                var companyTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                await Task.WhenAll(companyTask, specializationTask);

                _categories = companyTask.Result;
                _specializations = specializationTask.Result;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
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
                DynamicSearchFilter dynamicSearchFilter = new()
                {
                    Id = Model.Id,
                    CategoryId = Model.CategoryId,
                    SpecializationId = Model.Specializationid,
                    Key = Model.Key
                };

                bool isUpdated = false;
                var result = await DynamicSearchFilterService.UpdateAsync(dynamicSearchFilter);

                if (result.Id == Model.Id && result.Key == Model.Key)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere Søgeordet, grundet ukendt fejl.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditDynamicSearchFilter");
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
            Model = new DynamicSearchFilterModel();
            StateHasChanged();
        }

    }
}
