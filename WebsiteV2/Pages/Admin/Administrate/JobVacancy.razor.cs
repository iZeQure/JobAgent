using BlazorServerWebsite.Data.Services;
using Microsoft.AspNetCore.Components;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class JobVacancy
    {
        [Parameter] public int JobAdvertId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected JobAdvertService JobService { get; set; }
        //[Inject] protected SpecializationService Specialization { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        private CancellationTokenSource _tokenSource = new();
        private IEnumerable<JobAdvert> jobAdverts = new List<JobAdvert>();
        private JobAdvert AdvertDetails;
        private bool loadFailed;

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

            AdvertDetails = await JobService.GetByIdAsync(advertId, _tokenSource.Token);
        }

        private async Task LoadData()
        {
            try
            {
                loadFailed = false;

                if (JobAdvertId == 0 && SpecializationId == 0)
                {
                    //jobAdverts = await JobService.GetUncategorizedJobVacancies();
                }
                else if (SpecializationId == 0)
                {
                    jobAdverts = await JobService.GetAllAsync(_tokenSource.Token);
                }
                else if (SpecializationId != 0)
                {
                    //jobAdverts = await JobService.GetJobSpecialVacanciesAsync(SpecializationId);
                }
            }
            catch (Exception ex)
            {
                loadFailed = true;
                Console.WriteLine($"Failed to load Job Adverts : {ex.Message}");
            }
        }
    }
}
