using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.VacantJobModals
{
    public partial class CreateVacantJobModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private VacantJobModel _vacantJobModel = new();
        private IEnumerable<IVacantJob> _jobPages;
        private IEnumerable<ICompany> _companies;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;
        private bool _isProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_vacantJobModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformation();
        }

        private async Task LoadModalInformation()
        {
            _isLoading = true;

            try
            {
                var companyTask = CompanyService.GetAllAsync();
                var vacantJobTask = VacantJobService.GetAllAsync();

                try
                {
                    await TaskExtProvider.WhenAll(companyTask, vacantJobTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _companies = companyTask.Result;
                _jobPages = vacantJobTask.Result;
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

        private async Task OnValidSubmit_CreateJobAdvertAsync()
        {
            _isProcessing = true;
            try
            {
                if (!_vacantJobModel.URL.Contains("http://") || !_vacantJobModel.URL.Contains("https://"))
                {
                    _vacantJobModel.URL = "https://" + _vacantJobModel.URL;
                }

                VacantJob vacantJob = new()
                {
                    Id = _vacantJobModel.Id,
                    CompanyId = _vacantJobModel.CompanyId,
                    URL = _vacantJobModel.URL
                };

                bool isCreated = false;
                var result = await VacantJobService.CreateAsync(vacantJob);

                if (result.Id == _vacantJobModel.Id && result.URL == _vacantJobModel.URL)
                {
                    isCreated = true;
                }

                if (!isCreated)
                {
                    _errorMessage = "Kunne ikke oprette stilingsopslag grundet ukendt fejl";
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateVacantJob");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_vacantJobModel.Id}");

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
            _vacantJobModel = new();
            _editContext = new(_vacantJobModel);
            StateHasChanged();
        }
    }
}
