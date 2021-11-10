using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
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
        [Inject] protected IVacantJobService VacantJobService { get; set; }
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

    /// <summary>
    /// Represents a model which handles the pagination of the Job Advert page.
    /// </summary>
    public class JobAdvertPaginationModel
    {
        /// <summary>
        /// Initializes the model with default values.
        /// </summary>
        public JobAdvertPaginationModel(int maxContentPerPage = 25, int currentPage = 1)
        {
            JobAdverts = new();
            JobAdvertsPerPage = maxContentPerPage;
            CurrentPage = currentPage;
        }

        public List<IJobAdvert> JobAdverts { get; set; }
        public int JobAdvertsPerPage { get; set; } = 25;
        public int CurrentPage { get; set; } = 1;
        public IEnumerable<IJobAdvert> PaginatedJobAdverts
        {
            get
            {
                return CreatePagination();
            }
        }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(JobAdverts.Count / (double)JobAdvertsPerPage));
        }

        private IEnumerable<IJobAdvert> CreatePagination()
        {
            int start = (CurrentPage - 1) * JobAdvertsPerPage;

            return JobAdverts
                .OrderBy(j => j.Id)
                .Skip(start)
                .Take(JobAdvertsPerPage);
        }

        public void ResetToDefaultSettings()
        {
            JobAdverts = new();
            JobAdvertsPerPage = 25;
            CurrentPage = 1;
        }
    }
}
