﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PolicyLibrary.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.JobPageModals
{
    public partial class CreateJobPageModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobPageService JobPageService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private readonly DefaultValidator defaultValidator = new();
        private JobPageModel _jobPageModel = new();
        private IEnumerable<IJobPage> _jobPages;
        private IEnumerable<ICompany> _companies;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_jobPageModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var companyTask = CompanyService.GetAllAsync();
                var jobPageTask = JobPageService.GetJobPagesAsync();

                try
                {
                    await TaskExtProvider.WhenAll(companyTask, jobPageTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _companies = companyTask.Result;
                _jobPages = jobPageTask.Result;
            }
            catch (Exception)
            {
                _errorMessage = "Uventet fejl. Prøv at genindlæse siden.";
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit_CreateJobPageAsync()
        {
            if (_jobPageModel.IsProcessing is true)
            {
                return;
            }

            IJobPage result = null;

            using (var _ = _jobPageModel.TimedEndOfOperation())
            {
                if (_jobPageModel.CompanyId <= 0)
                {
                    _errorMessage = "Vælg et company for at tilføje link.";
                    return;
                }

                try
                {
                    if (!defaultValidator.ValidateUrl(_jobPageModel.URL))
                    {
                        _errorMessage = "Ikke en valid URL.";
                        return;
                    }
                }
                catch (Exception)
                {
                    _errorMessage = "Fejl i Jobsidens Link. Prøv igen eller tjek for fejl.";
                    return;
                }

                JobPage jobPage = new()
                {
                    Id = _jobPageModel.Id,
                    CompanyId = _jobPageModel.CompanyId,
                    URL = _jobPageModel.URL

                };

                result = await JobPageService.CreateAsync(jobPage);

                if (result is null)
                {
                    _errorMessage = "Fejl i oprettelse af jobside.";
                    return;
                }
            }

            if (_jobPageModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateJobPage");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{result.Id}");
            }
        }

        private void OnClick_CancelRequest()
        {
            _jobPageModel = new();
            _editContext = new(_jobPageModel);
            StateHasChanged();
        }
    }
}

