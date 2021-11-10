using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Administrate
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

        private JobAdvertModel _jobAdvertModel = new();
        private JobAdvertPaginationModel _paginationModel;
        private IEnumerable<ICategory> _categories;
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
                var paginatedResults = await JobAdvertService.JobAdvertPagination();
                var paginationModel = new JobAdvertPaginationModel()
                {
                    JobAdvertsPerPage = 25,
                    JobAdverts = paginatedResults,
                    CurrentPage = 1
                };
                var categories = await CategoryService.GetCategoriesAsync();
                var companies = await CompanyService.GetAllAsync();

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
            _paginationModel = new JobAdvertPaginationModel
            {
                CurrentPage = pageNumber,
                JobAdverts = await JobAdvertService.JobAdvertPagination(pageNumber),
                JobAdvertsPerPage = 25
            };
        }

        private async Task RefreshAsync()
        {
            try
            {
                var pagination = new JobAdvertPaginationModel
                {
                    JobAdverts = await JobAdvertService.JobAdvertPagination(),
                    CurrentPage = 1,
                    JobAdvertsPerPage = 25
                };

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

        public async Task OnClickEdit_GetJobAdvertDetailsById(int id)
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

        public void OnClick_StoreId(int id)
        {
            _advertId = id;
        }

        public async Task FilterContent(int page = 1)
        {
            if (_categoryId == 0)
            {
                _paginationModel = new JobAdvertPaginationModel
                {
                    JobAdverts = await JobAdvertService.JobAdvertPagination(page),
                    CurrentPage = page,
                    JobAdvertsPerPage = 25
                };
            }

            if (_categoryId != 0)
            {
                _paginationModel = new JobAdvertPaginationModel
                {
                    JobAdverts = await JobAdvertService.FilteredJobAdvertPagination(_categoryId, page),
                    CurrentPage = page,
                    JobAdvertsPerPage = 25
                };

                _filteredContentFound = _paginationModel.JobAdverts.Count() == 0 ? false : true;
            }
        }

        public async Task ClearFilteredContent()
        {
            _categoryId = 0;
            _paginationModel = new JobAdvertPaginationModel
            {
                JobAdverts = await JobAdvertService.JobAdvertPagination(),
                CurrentPage = 1,
                JobAdvertsPerPage = 25
            };
        }
    }

    public class JobAdvertModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg venligst en tilhører fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "* Vælg venligst en tilhører fra listen.")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en titel på stillingsopslaget.")]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en beskrivelse på stillingsopslaget.")]
        public string Summary { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Indtast en gyldig registrerings dato.")]
        public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow;

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg en kategori fra listen.")]
        [Range(0, int.MaxValue, ErrorMessage = "Der er ikke valgt nogen kategori.")]
        public int CategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "* Vælg et speciale fra listen.")]
        [Range(0, int.MaxValue, ErrorMessage = "Vælg et speciale til den valgte kategori.")]
        public int SpecializationId { get; set; } = 0;
    }

    public class JobAdvertPaginationModel
    {
        public List<IJobAdvert> JobAdverts { get; set; }
        public int JobAdvertsPerPage { get; set; }
        public int CurrentPage { get; set; } = 1;

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(JobAdverts.Count() / (double)JobAdvertsPerPage));
        }

        public IEnumerable<IJobAdvert> PaginatedJobAdverts()
        {
            int start = (CurrentPage - 1) * JobAdvertsPerPage;

            return JobAdverts.OrderBy(j => j.Id).Skip(start).Take(JobAdvertsPerPage);
        }
    }

}
