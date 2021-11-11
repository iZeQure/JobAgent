using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.JobAdverts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class JobAdvertPage : ComponentBase
    {
        [Parameter] public int JobAdvertId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        private JobAdvertPaginationModel _paginationModel = new();
        private JobAdvertModel _jobAdvertModel = new();
        private List<ICategory> _categories;
        private int _advertId = 0;
        private int _categoryId = 0;
        private bool _createJobAdvertBtnIsDisabled = false;
        private bool _filteredContentFound = false;
        private bool _dataIsLoading = false;
        private string _errorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshDataAsync;

            try
            {
                _dataIsLoading = true;

                _paginationModel.JobAdverts = await JobAdvertService.GetJobAdvertsAsync();
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

        private async Task RefreshDataAsync()
        {
            try
            {
                _paginationModel.ResetToDefaultSettings();
                _paginationModel.JobAdverts = (await JobAdvertService.GetJobAdvertsAsync())
                    .OrderBy(j => j.RegistrationDateTime)
                    .ToList();
            }
            catch (Exception) { _errorMessage = "Ukendt Fejl ved opdatering af stillingsopslag."; }
            finally { StateHasChanged(); }
        }

        public async Task OnButtonClick_EditJobAdvert_LoadJobAdvertDetails(int id)
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

        public async Task FilterJobAdverts(int page = 1)
        {
            if (_categoryId is 0)
            {
                _paginationModel.CurrentPage = page;
                _paginationModel.JobAdverts = (await JobAdvertService.GetJobAdvertsAsync())
                    .OrderBy(j => j.RegistrationDateTime)
                    .ToList();
            }
            else
            {
                _paginationModel.CurrentPage = page;
                _paginationModel.JobAdverts = (await JobAdvertService.GetJobAdvertsAsync())
                    .Where(j => j.CategoryId == _categoryId)
                    .OrderBy(j => j.Id)
                    .ToList();

                _filteredContentFound = _paginationModel.JobAdverts.Any();
            }
        }

        public async Task ClearFilteredContent()
        {
            _categoryId = 0;
            _paginationModel.ResetToDefaultSettings();
            _paginationModel.JobAdverts = await JobAdvertService.GetJobAdvertsAsync();
        }
    }
}
