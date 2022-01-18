using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using BlazorWebsite.Data.Services;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.RobotSettings
{
    public partial class VacantJobPage
    {
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private VacantJobModel _vacantJobModel = new();
        private IEnumerable<IVacantJob> _vacantJobs;
        private IEnumerable<ICompany> _companies;
        private IVacantJob _vacantJob;

        private int _vacantJobId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var jobPageTask = VacantJobService.GetAllAsync();
                var companyTask = CompanyService.GetAllAsync();

                await Task.WhenAll(jobPageTask, companyTask);

                _vacantJobs = jobPageTask.Result;
                _companies = companyTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _vacantJobId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _vacantJob = await VacantJobService.GetByIdAsync(id);

                _vacantJobModel = new VacantJobModel
                {
                    Id = _vacantJob.Id,
                    CompanyId = _vacantJob.CompanyId,
                    URL = _vacantJob.URL
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        public override async Task RefreshContent()
        {
            try
            {
                var links = await VacantJobService.GetAllAsync();

                if (links == null)
                {
                    return;
                }

                _vacantJobs = links;
            }
            finally
            {
                StateHasChanged();
            }
        }


    }
}
