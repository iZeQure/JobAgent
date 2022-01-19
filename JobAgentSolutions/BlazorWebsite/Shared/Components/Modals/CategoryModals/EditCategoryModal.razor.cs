using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Extensions;
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

        private ICategory _category;
        private List<string> _newSpecializationNames = new();

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnParametersSetAsync()
        {
            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                _category = await CategoryService.GetCategoryWithSpecializationsById(Model.CategoryId);
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
            if (Model.IsProcessing is true)
            {
                return;
            }

            try
            {
                using (var _ = Model.TimedEndOfOperation())
                {
                    Category category = new()
                    {
                        Id = Model.CategoryId,
                        Name = Model.Categoryname
                    };

                    bool specializationIsCreated = true;
                    var result = await CategoryService.UpdateAsync(category);

                    if (result is null)
                    {
                        _errorMessage = "Fejl under opdatering af Uddannelse.";
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

                            if (specializationIsCreated is false)
                            {
                                _errorMessage = "Fejl under opdatering af speciale.";
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                _errorMessage = "Noget gik galt.";
                return;
            }

            if (Model.IsProcessing is false)
            {
                _newSpecializationNames = new();
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCategory");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.CategoryId}");
            }
        }

        private void OnButtonClick_AssignNewSpecializationToList(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
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

        private async Task OnButtonClick_RemoveNewSpecialization(ISpecialization entity)
        {
            try
            {
                var result = await CategoryService.RemoveAsync(entity);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne specialet, det er muligvis allerede slettet.";
                }
            }
            catch (Exception)
            {
                _errorMessage = "Der skete en fejl.";
                return;
            }
            finally
            {
                RefreshProvider.CallRefreshRequest();
            }
        }

        private void OnClick_CancelRequest()
        {
            _newSpecializationNames = new();
            Model = new CategoryModel();
            StateHasChanged();
        }
    }
}
