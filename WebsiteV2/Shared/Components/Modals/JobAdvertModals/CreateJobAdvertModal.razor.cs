using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ObjectLibrary.Common;
using SecurityLibrary.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.JobAdvertModals
{
    public partial class CreateJobAdvertModal : ComponentBase
    {
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected ICategoryService CategoryService { get; set; }
        [Inject] protected ISpecializationService SpecializationService { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private JobAdvertModel _jobAdvertModel = new();
        private IEnumerable<VacantJob> _vacantJobs;
        private IEnumerable<Category> _categories;
        private IEnumerable<Specialization> _specializations;
        private IEnumerable<Specialization> _sortedSpecializations;
        private EditContext _editContext;

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
            _isLoading = true;

            try
            {
                var vacantJobsTask = VacantJobService.GetAllAsync(_tokenSource.Token);
                var categoriesTask = CategoryService.GetAllAsync(_tokenSource.Token);
                var specializationsTask = SpecializationService.GetAllAsync(_tokenSource.Token);

                try
                {
                    await TaskExtProvider.WhenAll(vacantJobsTask, categoriesTask, specializationsTask);
                }
                catch (Exception ex)
                {
                    _tokenSource.Cancel();
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _vacantJobs = vacantJobsTask.Result;
                _categories = categoriesTask.Result;
                _specializations = specializationsTask.Result;
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
            _editContext = new(_jobAdvertModel);
            StateHasChanged();
        }
    }
}
