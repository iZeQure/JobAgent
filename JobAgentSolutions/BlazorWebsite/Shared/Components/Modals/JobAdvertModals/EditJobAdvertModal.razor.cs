using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class EditJobAdvertModal : ComponentBase
    {
        [Parameter] public JobAdvertModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        public IEnumerable<ISpecialization> Specializations { get => _specializations; set => _specializations = value; }

        private IEnumerable<IVacantJob> _vacantJobs = new List<VacantJob>();
        private IEnumerable<ICategory> _categories = new List<Category>();
        private IEnumerable<ICompany> _companies = new List<Company>();
        private IEnumerable<ISpecialization> _specializations = new List<Specialization>();
        private List<ISpecialization> _sortedSpecializations = new List<ISpecialization>();
        private EditContext _editContext;
        private string companyName = string.Empty;

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
                var vacantJobsTask = VacantJobService.GetAllAsync();
                var categoriesTask = CategoryService.GetCategoriesAsync();
                var specializationsTask = CategoryService.GetSpecializationsAsync();
                var companyTask = CompanyService.GetAllAsync();

                await Task.WhenAll(vacantJobsTask, categoriesTask, specializationsTask, companyTask);

                _vacantJobs = vacantJobsTask.Result;
                _categories = categoriesTask.Result;
                Specializations = specializationsTask.Result;
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
                JobAdvert jobAdvert = new()
                {
                    Id = Model.Id,
                    CategoryId = Model.CategoryId,
                    SpecializationId = Model.SpecializationId,
                    Title = Model.Title,
                    Summary = Model.Summary,
                    RegistrationDateTime = Model.RegistrationDateTime

                };

                bool isUpdated = false;
                var result = await JobAdvertService.UpdateAsync(jobAdvert);

                if(result.Id == Model.Id && result.Title == Model.Title)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere stillingsopslag.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditJobAdvert");
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

        private void OnChange_SortSpecializationListByCategoryId(ChangeEventArgs e)
        {
            _sortedSpecializations.Clear();
            var parsed = int.TryParse(e.Value.ToString(), out int categoryId);

            if (parsed)
            {
                Model.CategoryId = categoryId;
                Model.SpecializationId = 0;

                foreach (var speciality in Specializations)
                {
                    if (speciality.CategoryId == categoryId)
                    {
                        _sortedSpecializations.Add(speciality);
                    }
                }

                StateHasChanged();
            }
        }

        private void OnClick_CancelRequest()
        {
            _sortedSpecializations.Clear();
            Model = new JobAdvertModel();
            _editContext = new(Model);
            StateHasChanged();
        }
    }
}
