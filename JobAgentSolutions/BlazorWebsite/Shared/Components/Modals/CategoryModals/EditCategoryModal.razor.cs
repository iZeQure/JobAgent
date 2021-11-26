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

                bool specializationIsCreated = true;
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

                if (_newSpecializationNames.Count > 0)
                {
                    foreach (var name in _newSpecializationNames)
                    {
                        Specialization specialization = new()
                        {
                            CategoryId = Model.CategoryId,
                            Name = name
                        };

                        var specializationResult = await CategoryService.CreateAsync(specialization);

                        if (specializationResult.CategoryId != specialization.CategoryId && specialization.Name != specializationResult.Name)
                        {
                            specializationIsCreated = false;
                        }
                    }
                }

                if (!specializationIsCreated)
                {
                    _errorMessage = "Kunne ikke oprette speciale(r) grundet ukendt fejl.";
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
                _newSpecializationNames = new();
                _specializations = await CategoryService.GetSpecializationsAsync();
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
            }

            _specializations = await CategoryService.GetSpecializationsAsync();

            StateHasChanged();

            _isProcessingNewSpecializationToList = false;
        }

        private void OnClick_CancelRequest()
        {
            _newSpecializationNames = new();
            Model = new CategoryModel();
            StateHasChanged();
        }
    }
}
