using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Administrate
{
    public partial class JobPagePage : ComponentBase
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJobPageService JobPageService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private IEnumerable<IJobPage> _jobPages;
        private IEnumerable<ICompany> _companies;
        private IJobPage _jobPage;

        private int jobPageId = 0;
        private bool disabled = false;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshContent;

            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                _jobPages = await JobPageService.GetJobPagesAsync();
                _companies = await CompanyService.GetAllAsync();
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            jobPageId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            _jobPage = await JobPageService.GetByIdAsync(id);
        }

        private async Task RefreshContent()
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
