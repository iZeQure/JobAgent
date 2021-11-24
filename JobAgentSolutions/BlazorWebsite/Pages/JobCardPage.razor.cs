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
    public partial class JobCardPage : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("da-DK");
        private IEnumerable<IJobAdvert> _jobAdverts = new List<JobAdvert>();
        private IEnumerable<IVacantJob> _vacantJobs = new List<VacantJob>();
        private IEnumerable<ICompany> _companies = new List<Company>();

        private bool _isLoadingData;
        private bool _loadFailed;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            _isLoadingData = true;
            try
            {
                _loadFailed = false;

                _vacantJobs = await VacantJobService.GetAllAsync();
                _companies = await CompanyService.GetAllAsync();
                var jobAdverts = await JobAdvertService.GetJobAdvertsAsync();

                if (CategoryId == 0 && SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == 0 && x.SpecializationId == 0);
                }
                else if (SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == CategoryId);
                }
                else if (SpecializationId != 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == SpecializationId);
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
    }
}
