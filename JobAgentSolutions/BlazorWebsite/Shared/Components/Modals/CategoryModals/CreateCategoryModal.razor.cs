using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
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

        private string _errorMessage = "";
        private bool _isProcessingNewSpecializationToList = false;
        private bool _isLoading = false;
        private bool _isProcessing = false;

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
            _isProcessing = true;
            try
            {
                Category category = new()
                {
                    Name = _categoryModel.Categoryname
                };

                bool categoryIsCreated = false;
                bool specializationIsCreated = true;
                var categoryResult = await CategoryService.CreateAsync(category);

                if (categoryResult.Name == _categoryModel.Categoryname)
                {
                    categoryIsCreated = true;
                    _categoryModel.CategoryId = categoryResult.Id;
                }

                if (!categoryIsCreated)
                {
                    _errorMessage = "Kunne ikke oprette uddannelsen grundet ukendt fejl.";
                }

                foreach(var name in _newSpecializationNames)
                {
                    Specialization specialization = new()
                    {
                        CategoryId = _categoryModel.CategoryId,
                        Name = name
                    };

                    var specializationResult = await CategoryService.CreateAsync(specialization);

                    if(specializationResult.CategoryId != specialization.CategoryId && specialization.Name != specializationResult.Name)
                    {
                        specializationIsCreated = false;
                    }
                }

                if (!specializationIsCreated)
                {
                    _errorMessage = "Kunne ikke oprette specialer grundet ukendt fejl.";
                }


                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCategory");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_categoryModel.CategoryId}");

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
            _newSpecializationNames.Add(name);

            StateHasChanged();

            return Task.CompletedTask;
        }

        private void OnClick_CancelRequest()
        {
            _categoryModel = new();
            _editContext = new(_categoryModel);
            StateHasChanged();
        }
    }
}
