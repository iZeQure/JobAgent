﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
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
    public partial class JobPagePage : ComponentBase
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
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
            RefreshProvider.RefreshRequest += RefreshContent;

            await LoadData();
        }

        private async Task LoadData()
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

        private async Task OnClick_EditLink(int id)
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
            catch(Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
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