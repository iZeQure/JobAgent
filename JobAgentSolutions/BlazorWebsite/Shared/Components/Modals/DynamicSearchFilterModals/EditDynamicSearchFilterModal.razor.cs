﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool _isLoading = false;
        private bool _hasSpecs;

        protected override async Task OnInitializedAsync()
        {
            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var categoryTask = CategoryService.GetCategoriesAsync();
                var specializationTask = CategoryService.GetSpecializationsAsync();

                await Task.WhenAll(categoryTask, specializationTask);

                _categories = categoryTask.Result;
                _specializations = specializationTask.Result;

                await DetermineIfAnySpezializationsAsync();
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

        private async Task OnValidSubmit_EditDynamicSearchFilterAsync()
        {
            if (Model.IsProcessing is true)
            {
                return;
            }
            using (var _ = Model.TimedEndOfOperation())
            {
                DynamicSearchFilter dynamicSearchFilter = new()
                {
                    Id = Model.Id,
                    CategoryId = Model.CategoryId,
                    SpecializationId = Model.Specializationid,
                    Key = Model.Key
                };

                var result = await DynamicSearchFilterService.UpdateAsync(dynamicSearchFilter);

                if (result is null)
                {
                    _errorMessage = "Fejl under opdatering af filteret.";
                    return;
                }
            }

            if (Model.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditDynamicSearchFilter");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
        }

        private async Task OnChange_CheckCategorySpecializationsAsync(ChangeEventArgs e)
        {
            Model.CategoryId = int.Parse(e.Value.ToString());
            await DetermineIfAnySpezializationsAsync();
        }

        private async Task DetermineIfAnySpezializationsAsync()
        {
            ICategory category = await CategoryService.GetCategoryWithSpecializationsById(Model.CategoryId);

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
            Model = new DynamicSearchFilterModel();
            StateHasChanged();
        }

    }
}
