using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class JobPagePage : ComponentBase
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJobPageService JobPageService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private IEnumerable<JobPage> _jobPages;
        private IEnumerable<Company> _companies;
        private JobPage _jobPage;

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
                _jobPages = await JobPageService.GetAllAsync(_tokenSource.Token);
                _companies = await CompanyService.GetAllAsync(_tokenSource.Token);
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
            _jobPage = await JobPageService.GetByIdAsync(id, _tokenSource.Token);
        }

        private async Task RefreshContent()
        {
            try
            {
                var links = await JobPageService.GetAllAsync(_tokenSource.Token);

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
