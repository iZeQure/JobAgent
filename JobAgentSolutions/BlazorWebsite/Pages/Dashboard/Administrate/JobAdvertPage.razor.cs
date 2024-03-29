﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Services;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class JobAdvertPage
    {
        [Parameter] public int JobAdvertId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected PaginationService PaginationService { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private JobAdvertModel _jobAdvertModel = new();
        private List<ICategory> _categories;
        private List<IJobAdvert> _jobAdverts;

        private int _advertId = 0;
        private int _categoryId = 0;
        private string _categoryName = string.Empty;
        private bool _createJobAdvertBtnIsDisabled = false;
        private bool _filteredContentFound = false;
        private bool _dataIsLoading = false;
        private string _errorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                _dataIsLoading = true;

                _jobAdverts = await JobAdvertService.GetJobAdvertsAsync();
                _categories = await CategoryService.GetCategoriesAsync();

                var company = (await CompanyService.GetAllAsync()).FirstOrDefault();

                if (company is null)
                {
                    _createJobAdvertBtnIsDisabled = true;
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _dataIsLoading = false;
            }
        }

        public override async Task RefreshContent()
        {
            try
            {
                PaginationService.ResetDefaults();
                _jobAdverts = (await JobAdvertService.GetJobAdvertsAsync())
                    .OrderBy(j => j.RegistrationDateTime)
                    .ToList();
            }
            catch (Exception) { _errorMessage = "Ukendt Fejl ved opdatering af stillingsopslag."; }
            finally { StateHasChanged(); }
        }

        public async Task OnButtonClick_EditJobAdvert_LoadJobAdvertDetailsAsync(int id)
        {
            var details = await JobAdvertService.GetByIdAsync(id);

            _jobAdvertModel = new JobAdvertModel()
            {
                Id = details.Id,
                Title = details.Title,
                RegistrationDateTime = details.RegistrationDateTime,
                Summary = details.Summary,
                CategoryId = details.CategoryId,
                SpecializationId = details.SpecializationId
            };
        }

        public void OnButtonClick_RemoveJobAdvert_StoreId(int id)
        {
            _advertId = id;
        }

        public async Task FilterJobAdvertsAsync()
        {
            _jobAdverts = (await JobAdvertService.GetJobAdvertsAsync())
                .Where(j => j.CategoryId == _categoryId)
                .OrderBy(j => j.Id)
                .ToList();

            _filteredContentFound = _jobAdverts.Any();
        }

        public async Task ClearFilteredContentAsync()
        {
            _categoryId = 0;
            PaginationService.ResetDefaults();
            _jobAdverts = await JobAdvertService.GetJobAdvertsAsync();
        }
    }
}
