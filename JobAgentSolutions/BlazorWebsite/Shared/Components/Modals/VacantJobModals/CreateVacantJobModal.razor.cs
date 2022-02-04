using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using PolicyLibrary.Validators;
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

        private readonly DefaultValidator defaultValidator = new();
        private VacantJobModel _vacantJobModel = new();
        private IEnumerable<IVacantJob> _jobPages;
        private IEnumerable<ICompany> _companies;
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_vacantJobModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
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
            if (_vacantJobModel.IsProcessing is true)
            {
                return;
            }

            IVacantJob result = null;

            using (var _ = _vacantJobModel.TimedEndOfOperation())
            {
                if (_vacantJobModel.CompanyId <= 0)
                {
                    _errorMessage = "Vælg et company for at tilføje link.";
                    return;
                }

                try
                {
                    if (!defaultValidator.ValidateUrl(_vacantJobModel.URL))
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

                VacantJob vacantJob = new()
                {
                    Id = _vacantJobModel.Id,
                    CompanyId = _vacantJobModel.CompanyId,
                    URL = _vacantJobModel.URL
                };

                result = await VacantJobService.CreateAsync(vacantJob);

                if (result is null)
                {
                    _errorMessage = "Fejl i oprettelse af stilling.";
                    return;
                }
            }

            if (_vacantJobModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateVacantJob");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{result.Id}");
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
