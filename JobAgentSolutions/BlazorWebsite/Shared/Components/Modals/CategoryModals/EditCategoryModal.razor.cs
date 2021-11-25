using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CategoryModals
{
    public partial class EditCategoryModal : ComponentBase
    {
        [Parameter] public CategoryModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private IEnumerable<ICategory> _categories = new List<Category>();
        private IEnumerable<ISpecialization> _specializations = new List<Specialization>();
        private List<string> _newSpecializationNames = new();
        private List<string> _oldSpecializationNames = new();


        private string _errorMessage = "";
        private bool _isProcessing = false;
        private bool _isProcessingNewSpecializationToList = false;
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
                Category category = new()
                {
                    Id = Model.CategoryId,
                    Name = Model.Categoryname
                };

                bool isUpdated = false;
                var result = await CategoryService.UpdateAsync(category);

                if (result.Id == Model.CategoryId && result.Name == Model.Categoryname)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere Uddannelsen, grundet ukendt fejl.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCategory");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.CategoryId}");
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
        private Task OnButtonClick_AssignNewSpecializationToList(string name)
        {
            _isProcessingNewSpecializationToList = true;

            _newSpecializationNames.Add(name);

            StateHasChanged();

            _isProcessingNewSpecializationToList = false;

            return Task.CompletedTask;
        }

        private async Task OnButtonClick_RemoveNewSpecialization(ISpecialization entity)
        {
            _isProcessingNewSpecializationToList = true;

            var result = await CategoryService.RemoveAsync(entity);

            if (!result)
            {
                _errorMessage = "Kunne ikke fjerne specialet, det er muligvis allerede slettet.";

                return;
            }

            _isProcessingNewSpecializationToList = false;
            
            StateHasChanged();
        }

        private void OnClick_CancelRequest()
        {
            Model = new CategoryModel();
            StateHasChanged();
        }


    }
}
