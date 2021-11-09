using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages
{
    public partial class JobCardPage : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private IEnumerable<JobAdvert> _jobAdverts = new List<JobAdvert>();
        private JobAdvert _jobAdvertDetails;

        private int _chosenCategoryId;
        private bool _isLoadingData = false;
        private bool _loadFailed;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
        
        protected override async Task OnParametersSetAsync()
        {
            await LoadData();
        }

        private async Task GetJobAdvertDetails(int advertId)
        {
            if (advertId == 0)
            {
                await Task.CompletedTask;
            }

            _jobAdvertDetails = await JobAdvertService.GetByIdAsync(advertId, _tokenSource.Token);
        }

        private async Task LoadData()
        {
            try
            {
                _loadFailed = false;
                var jobAdverts = await JobAdvertService.GetAllAsync(_tokenSource.Token);

                if (CategoryId == 0 && SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.Category.Id == 0 && x.Specialization.Id == 0);
                }
                else if (SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.Category.Id == CategoryId);
                }
                else if (SpecializationId != 0)
                {
                    jobAdverts = jobAdverts.Where(x => x.Category.Id == SpecializationId);
                }
            }
            catch (Exception ex)
            {
                _loadFailed = true;
                Console.WriteLine($"Failed to load Job Adverts : {ex.Message}");
            }
        }
    }
}
