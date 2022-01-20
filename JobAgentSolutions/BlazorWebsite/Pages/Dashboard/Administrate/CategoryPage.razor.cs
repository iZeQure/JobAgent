using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Services;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    partial class CategoryPage
    {
        [Inject] protected ICategoryService CategoryService { get; set; }

        private CategoryModel _categoryModel = new();
        private IEnumerable<ICategory> _categories;
        private IEnumerable<ISpecialization> _specializations;
        private ICategory _category;

        private int _categoryId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var categoryTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                await Task.WhenAll(categoryTask, specializationTask);

                _categories = categoryTask.Result;
                _specializations = specializationTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _categoryId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _category = await CategoryService.GetCategoryByIdAsync(id);

                _categoryModel = new CategoryModel
                {
                    CategoryId = _category.Id,
                    Categoryname = _category.Name,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        public override async Task RefreshContent()
        {
            try
            {
                var cats = await CategoryService.GetCategoriesAsync();
                var specs = await CategoryService.GetSpecializationsAsync();

                if (cats is null || specs is null)
                {
                    return;
                }

                _categories = cats;
                _specializations = specs;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
