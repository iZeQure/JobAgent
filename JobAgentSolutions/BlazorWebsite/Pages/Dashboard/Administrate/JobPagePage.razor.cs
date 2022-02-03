using BlazorWebsite.Data.FormModels;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class JobPagePage
    {
        [Inject] protected IJobPageService JobPageService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private JobPageModel _jobPageModel = new();
        private IEnumerable<IJobPage> _jobPages;
        private IEnumerable<ICompany> _companies;
        private IJobPage _jobPage;

        private int _jobPageId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            dataIsLoading = true;
            try
            {
                var jobPageTask = JobPageService.GetJobPagesAsync();
                var companyTask = CompanyService.GetAllAsync();

                await Task.WhenAll(jobPageTask, companyTask);

                _jobPages = jobPageTask.Result;
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
            _jobPageId = id;
        }

        private async Task OnClick_EditLinkAsync(int id)
        {
            try
            {
                _jobPage = await JobPageService.GetByIdAsync(id);

                _jobPageModel = new JobPageModel
                {
                    Id = _jobPage.Id,
                    CompanyId = _jobPage.CompanyId,
                    URL = _jobPage.URL
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
                var links = await JobPageService.GetJobPagesAsync();

                if (links == null)
                {
                    return;
                }

                _jobPages = links;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
