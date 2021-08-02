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
    public partial class CreateJobAdvertModal : ComponentBase
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IJobAdvertService JobAdvertService { get; set; }
        [Inject] private IVacantJobService VacantJobService { get; set; }
        [Inject] private ICategoryService CategoryService { get; set; }
        [Inject] private ISpecializationService SpecializationService { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private JobAdvertModel _jobAdvertModel = new();
        private EditContext _editContext;
        private IEnumerable<VacantJob> _vacantJobs;
        private IEnumerable<Category> _categories;
        private IEnumerable<Specialization> _specializations;
        private IEnumerable<Specialization> _sortedSpecializations;

        private string _errorMessage = "";
        private bool _isLoading = false;
        private bool _isProcessing = false;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_jobAdvertModel);
            _editContext.AddDataAnnotationsValidation();

            await LoadModalInformation();
        }

        private async Task LoadModalInformation()
        {
            _jobAdvertModel = new();

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

        private async Task OnValidSubmit_CreateJobAdvertAsync()
        {
            try
            {
                JobAdvert jobAdvert = new(
                    new VacantJob(
                        _jobAdvertModel.Id,
                        null,
                        ""),
                    new Category(_jobAdvertModel.CategoryId, ""),
                    new Specialization(_jobAdvertModel.SpecializationId, null, ""),
                    _jobAdvertModel.Title, _jobAdvertModel.Summary, _jobAdvertModel.RegistrationDateTime
                    );

                var result = await JobAdvertService.CreateAsync(jobAdvert, _tokenSource.Token);

                if (result == 1)
                {
                    RefreshProvider.CallRefreshRequest();
                    await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateJobAdvert");
                    return;
                }

                _errorMessage = "Kunne ikke oprette stilingsopslag!";
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }

        private void OnChange_GetSpecializationByCategoryId(ChangeEventArgs e)
        {
            _sortedSpecializations = Enumerable.Empty<Specialization>();

            var parsed = int.TryParse(e.Value.ToString(), out int categoryId);

            try
            {
                if (parsed)
                {
                    _jobAdvertModel.CategoryId = categoryId;
                    _jobAdvertModel.SpecializationId = 0;

                    var tempList = new List<Specialization>();

                    for (int i = 0; i < _specializations.Count(); i++)
                    {
                        tempList.Add(_specializations.ElementAt(i));
                    }

                    _sortedSpecializations = tempList;
                }
            }
            finally
            {
                StateHasChanged();
            }
        }

        private void OnClick_CancelRequest()
        {
            _tokenSource.Cancel();

            _sortedSpecializations = Enumerable.Empty<Specialization>();
            _jobAdvertModel = new();
            StateHasChanged();
        }
    }
}
