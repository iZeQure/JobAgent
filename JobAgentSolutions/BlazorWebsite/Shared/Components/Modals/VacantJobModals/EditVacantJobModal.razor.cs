﻿using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.VacantJobModals
{
    public partial class EditVacantJobModal : ComponentBase
    {
        [Parameter] public VacantJobModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IVacantJobService JobPageService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private IEnumerable<ICompany> _companies = new List<Company>();

        private string _errorMessage = "";
        private bool _isProcessing = false;
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var companyTask = CompanyService.GetAllAsync();

                await Task.WhenAll(companyTask);

                _companies = companyTask.Result;
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit_EditJobVacancy()
        {
            _isProcessing = true;

            try
            {
                if (!Model.URL.Contains("http://") || !Model.URL.Contains("https://"))
                {
                    Model.URL = "https://" + Model.URL;
                }

                VacantJob vacantJob = new()
                {
                    Id = Model.Id,
                    CompanyId = Model.CompanyId,
                    URL = Model.URL
                };

                bool isUpdated = false;
                var result = await JobPageService.UpdateAsync(vacantJob);

                if (result.Id == Model.Id && result.URL == Model.URL)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere stillingsopslaget.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditVacantJob");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new VacantJobModel();
            StateHasChanged();
        }
    }
}
