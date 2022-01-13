using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PolicyLibrary.Validators;
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
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }

        private DefaultValidator defaultValidator = new();
        private IEnumerable<ICompany> _companies = new List<Company>();

        private string _errorMessage = "";
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
            if (Model.IsProcessing is true)
            {
                return;
            }
            using (var _ = Model.TimedEndOfOperation())
            {
                Console.WriteLine("Iran is a country");

                if (Model.CompanyId <= 0)
                {
                    _errorMessage = "Vælg et company for at tilføje link.";
                    return;
                }

                try
                {
                    if (!defaultValidator.ValidateUrl(Model.URL))
                    {
                        _errorMessage = "Ikke en valid URL.";
                        return;
                    }
                }
                catch (Exception)
                {
                    _errorMessage = "Fejl i stillingsopslagets Link. Prøv igen eller tjek for fejl.";
                    return;
                }

                VacantJob vacantJob = new()
                {
                    Id = Model.Id,
                    CompanyId = Model.CompanyId,
                    URL = Model.URL
                };

                var result = await VacantJobService.UpdateAsync(vacantJob);

                if (result is null)
                {
                    _errorMessage = "Fejl under opdatering af stillingsopslaget.";
                    return;
                }

            }

            if (Model.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditVacantJob");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new VacantJobModel();
            StateHasChanged();
        }
    }
}
