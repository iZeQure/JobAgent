﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class CreateJobAdvertModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private EditContext _editContext;
        private JobAdvertModel _jobAdvertModel = new();
        private IEnumerable<ICompany> _companies;
        private IEnumerable<ICategory> _categories;
        private IEnumerable<IVacantJob> _vacantJobs;
        private IEnumerable<ISpecialization> _specializations;
        private IEnumerable<ISpecialization> _sortedSpecializations;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_jobAdvertModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var companyTask = CompanyService.GetAllAsync();
                var vacantJobsTask = VacantJobService.GetAllAsync();
                var categoriesTask = CategoryService.GetCategoriesAsync();
                var specializationsTask = CategoryService.GetSpecializationsAsync();

                try
                {
                    await TaskExtProvider.WhenAll(vacantJobsTask, categoriesTask, specializationsTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _companies = companyTask.Result;
                _vacantJobs = vacantJobsTask.Result;
                _categories = categoriesTask.Result;
                _specializations = specializationsTask.Result;
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
            if (_jobAdvertModel.IsProcessing is true)
            {
                return;
            }

            IJobAdvert result = null;

            using (var _ = _jobAdvertModel.TimedEndOfOperation())
            {
                JobAdvert jobAdvert = new()
                {
                    Id = _jobAdvertModel.Id,
                    CategoryId = _jobAdvertModel.CategoryId,
                    SpecializationId = _jobAdvertModel.SpecializationId,
                    Title = _jobAdvertModel.Title,
                    Summary = _jobAdvertModel.Summary,
                    RegistrationDateTime = _jobAdvertModel.RegistrationDateTime
                };

                result = await JobAdvertService.CreateAsync(jobAdvert);

                if (result is null)
                {
                    _errorMessage = "Fejl i oprettelse af JobOpslag.";
                    return;
                }
            }

            if (_jobAdvertModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateJobAdvert");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{result.Id}");
            }
        }

        private void OnChange_GetSpecializationByCategoryId(ChangeEventArgs e)
        {
            _sortedSpecializations = Enumerable.Empty<Specialization>();

            var parsed = int.TryParse(e.Value.ToString(), out int categoryId);

            try
            {
                if (parsed)
                {
                    _jobAdvertModel.CategoryId = categoryId;
                    _jobAdvertModel.SpecializationId = 0;

                    var tempList = new List<ISpecialization>();

                    for (int i = 0; i < _specializations.Count(); i++)
                    {
                        if (_specializations.ElementAt(i).CategoryId.Equals(categoryId))
                        {
                            tempList.Add(_specializations.ElementAt(i));
                        }
                    }

                    _sortedSpecializations = tempList;
                }
            }
            finally
            {
                StateHasChanged();
            }
        }

        private void OnClick_CancelRequest()
        {
            _sortedSpecializations = Enumerable.Empty<Specialization>();
            _jobAdvertModel = new();
            _editContext = new(_jobAdvertModel);
            StateHasChanged();
        }
    }
}
