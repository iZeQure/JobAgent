using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages
{
    public partial class JobCardPage
    {
        [Parameter] public int CategoryId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private readonly CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("da-DK");
        private IEnumerable<IJobAdvert> _jobAdverts = new List<JobAdvert>();
        private IEnumerable<IVacantJob> _vacantJobs = new List<VacantJob>();
        private IEnumerable<ICompany> _companies = new List<Company>();

        private bool _isLoadingData;
        private bool _loadFailed;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            _isLoadingData = true;
            try
            {
                _loadFailed = false;

                _vacantJobs = await VacantJobService.GetAllAsync();
                _companies = await CompanyService.GetAllAsync();
                var jobAdverts = await JobAdvertService.GetJobAdvertsAsync();

                if (CategoryId is 0 && SpecializationId is 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId is 0 && x.SpecializationId is 0);
                }
                else if (SpecializationId is 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == CategoryId);
                }
                else if (SpecializationId is not 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.SpecializationId == SpecializationId);
                }
            }
            catch (Exception ex)
            {
                _loadFailed = true;
                Console.WriteLine($"Failed to load Job Adverts : {ex.Message}");
            }
            finally
            {
                _isLoadingData = false;
            }
        }

        public async override Task RefreshContent()
        {
            await LoadDataAsync();
        }
    }
}
