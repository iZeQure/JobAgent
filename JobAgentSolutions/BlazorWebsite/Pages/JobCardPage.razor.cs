using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages
{
    public partial class JobCardPage : ComponentBase
    {
        [Parameter] public int CategoryId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private IEnumerable<IJobAdvert> _jobAdverts = new List<JobAdvert>();
        private IJobAdvert _jobAdvertDetails;

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

            _jobAdvertDetails = await JobAdvertService.GetByIdAsync(advertId);
        }

        private async Task LoadData()
        {
            try
            {
                _loadFailed = false;
                var jobAdverts = await JobAdvertService.GetJobAdvertsAsync();

                if (CategoryId == 0 && SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == 0 && x.SpecializationId == 0);
                }
                else if (SpecializationId == 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == CategoryId);
                }
                else if (SpecializationId != 0)
                {
                    _jobAdverts = jobAdverts.Where(x => x.CategoryId == SpecializationId);
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
