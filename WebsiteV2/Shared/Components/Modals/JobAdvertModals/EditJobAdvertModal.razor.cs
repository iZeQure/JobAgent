using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class EditJobAdvertModal : ComponentBase
    {
        [Parameter] public JobAdvertModel Model { get; set; }

        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ISpecializationService SpecializationService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private IEnumerable<VacantJob> _vacantJobs = new List<VacantJob>();
        private IEnumerable<Category> _categories = new List<Category>();
        private IEnumerable<Specialization> _specializations = new List<Specialization>();
        private IList<Specialization> _sortedSpecializations = new List<Specialization>();
        private EditContext _editContext;

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
                var vacantJobsTask = VacantJobService.GetAllAsync(_tokenSource.Token);
                var categoriesTask = CategoryService.GetAllAsync(_tokenSource.Token);
                var specializationsTask = SpecializationService.GetAllAsync(_tokenSource.Token);

                await Task.WhenAll(vacantJobsTask, categoriesTask, specializationsTask);

                _vacantJobs = vacantJobsTask.Result;
                _categories = categoriesTask.Result;
                _specializations = specializationsTask.Result;
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
                JobAdvert jobAdvert = new(
                    new VacantJob(Model.Id, null, ""),
                    new Category(Model.CategoryId, ""),
                    new Specialization(Model.SpecializationId, null, ""),
                    Model.Title,
                    Model.Summary,
                    Model.RegistrationDateTime
                    );

                var result = await JobAdvertService.UpdateAsync(jobAdvert, _tokenSource.Token);

                if (result == 0)
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

                foreach (var speciality in _specializations)
                {
                    if (speciality.Category.Id == categoryId)
                    {
                        _sortedSpecializations.Add(speciality);
                    }
                }

                StateHasChanged();
            }
        }

        private void OnClick_CancelRequest()
        {
            _tokenSource.Cancel();

            _sortedSpecializations.Clear();
            Model = new JobAdvertModel();
            _editContext = new(Model);
            StateHasChanged();
        }
    }
}
