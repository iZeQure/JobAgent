using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class JobAdvertPage : ComponentBase
    {
        [Parameter] public int JobAdvertId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJobAdvertService JobService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private JobAdvertModel _jobAdvertModel;
        private JobAdvertPaginationModel _paginationModel;
        private IEnumerable<Category> _categories;
        private int _advertId = 0;
        private int _categoryId = 0;
        private bool _isDisabled = false;
        private bool _filteredContentFound = false;
        private bool _dataIsLoading = false;
        private string _errorMessage = "";

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshAsync;

            await LoadData();
        }

        private async Task LoadData()
        {
            _dataIsLoading = true;

            try
            {
                var paginationModel = await JobService.JobAdvertPagination(_tokenSource.Token);
                var categories = await CategoryService.GetAllAsync(_tokenSource.Token);
                var companies = await CompanyService.GetAllAsync(_tokenSource.Token);

                if (companies.Any() == false)
                {
                    _isDisabled = true;
                }

                if (paginationModel != null && categories != null)
                {
                    _paginationModel = paginationModel;
                    _categories = categories;
                }
            }
            finally
            {
                _dataIsLoading = false;
            }
        }

        private async Task ReturnPage(int pageNumber)
        {
            _paginationModel = await JobService.JobAdvertPagination(_tokenSource.Token, pageNumber);
        }

        private async Task RefreshAsync()
        {
            try
            {
                var pagination = await JobService.JobAdvertPagination(_tokenSource.Token);

                if (pagination != null)
                {
                    _paginationModel = pagination;
                    return;
                }

                _paginationModel = null;

                _errorMessage = "Kunne ikke indlæse stillingsopslag.";
            }
            catch (Exception) { _errorMessage = "Ukendt Fejl ved opdatering af stillingsopslag."; }
            finally { StateHasChanged(); }
        }

        public async Task OnClickEdit_GetJobVacancyDetailsById(int id)
        {
            var details = await JobService.GetByIdAsync(id, _tokenSource.Token);

            _jobAdvertModel = new JobAdvertModel()
            {
                Id = details.Id,
                Title = details.Title,
                RegistrationDateTime = details.RegistrationDateTime,
                CategoryId = details.Category.Id,
                SpecializationId = details.Specialization.Id
            };
        }

        public void OnClick_StoreId(int id)
        {
            _advertId = id;
        }

        public async Task FilterContent(int page = 1)
        {
            if (_categoryId == 0)
            {
                _paginationModel = await JobService.JobAdvertPagination(_tokenSource.Token, page);
            }

            if (_categoryId != 0)
            {
                _paginationModel = await JobService.FilteredJobAdvertPagination(_tokenSource.Token, _categoryId, page);

                _filteredContentFound = _paginationModel.JobAdverts.Count() == 0 ? false : true;
            }
        }

        public async Task ClearFilteredContent()
        {
            _categoryId = 0;
            _paginationModel = await JobService.JobAdvertPagination(_tokenSource.Token);
        }
    }
}
