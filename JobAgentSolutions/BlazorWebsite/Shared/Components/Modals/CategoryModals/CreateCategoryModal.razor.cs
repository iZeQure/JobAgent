using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CategoryModals
{
    public partial class CreateCategoryModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }

        private CategoryModel _categoryModel = new();
        private IEnumerable<ICategory> _categories;
        private IEnumerable<ISpecialization> _specializations;
        private IEnumerable<ISpecialization> _newSpecializations = new List<Specialization>();
        private List<string> _newSpecializationNames = new();
        private EditContext _editContext;

        private bool _isProcessingNewSpecializationToList = false;
        private bool _isLoading = false;
        private string _errorMessage;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_categoryModel);
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
            if (_categoryModel.IsProcessing is true)
            {
                return;
            }

            try
            {
                using (var _ = _categoryModel.TimedEndOfOperation())
                {
                    ICategory categoryResult = null;

                    Category category = new()
                    {
                        Name = _categoryModel.Categoryname
                    };

                    categoryResult = await CategoryService.CreateAsync(category);

                    if (categoryResult is null)
                    {
                        _errorMessage = "Fejl i oprettelse af Uddannelse.";
                        return;
                    }
                    _categoryModel.CategoryId = categoryResult.Id;

                    foreach (var name in _newSpecializationNames)
                    {
                        Specialization specialization = new()
                        {
                            CategoryId = _categoryModel.CategoryId,
                            Name = name
                        };

                        var specializationResult = await CategoryService.CreateAsync(specialization);

                        if (specializationResult is null)
                        {
                            _errorMessage = "Fejl i oprettelse af Speciale.";
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
                _errorMessage = "Noget gik galt.";
                return;
            }

            if (_categoryModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCategory");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_categoryModel.CategoryId}");
            }
        }

        private void OnButtonClick_AssignNewSpecializationToList(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _errorMessage = "Udfyld felt før du trykker på knappen.";
                return;
            }

            if (_newSpecializationNames.Contains(name))
            {
                _errorMessage = "Du har allerede tilføjet dette speciale.";
                return;
            }

            _newSpecializationNames.Add(name);
            StateHasChanged();
        }

        private void OnClick_CancelRequest()
        {
            _newSpecializationNames = new();
            _categoryModel = new();
            _editContext = new(_categoryModel);
            StateHasChanged();
        }
    }
}
