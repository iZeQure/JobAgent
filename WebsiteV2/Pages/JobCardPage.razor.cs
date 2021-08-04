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
        [Parameter] public int JobAdvertId { get; set; }
        [Parameter] public int SpecializationId { get; set; }
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private CancellationTokenSource _tokenSource;
        private IEnumerable<VacantJob> _vacantJobs;
        private IEnumerable<JobAdvert> _jobAdverts;
        private VacantJobModel _vacantJobModel;

        private int chosenCategoryId;
        private List<(int, string, string, string, DateTime, string, int)> cards = new List<(int, string, string, string, DateTime, string, int)>();
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            _tokenSource = new();
            _vacantJobModel = new();
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

            AdvertDetails = await JobService.GetJobVacancyById(advertId);
        }

        private async Task LoadData()
        {
            try
            {
                loadFailed = false;

                if (JobAdvertId == 0 && SpecializationId == 0)
                {
                    jobAdverts = await JobService.GetUncategorizedJobVacancies();
                }
                else if (SpecializationId == 0)
                {
                    jobAdverts = await JobService.GetJobVacanciesAsync(JobAdvertId);
                }
                else if (SpecializationId != 0)
                {
                    jobAdverts = await JobService.GetJobSpecialVacanciesAsync(SpecializationId);
                }
            }
            catch (Exception ex)
            {
                loadFailed = true;
                Console.WriteLine($"Failed to load Job Adverts : {ex.Message}");
            }
        }








        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            var tuplelist = new List<(int, string, string, string, DateTime, string)>();

            if(firstRender)
            {
                _isLoadingData = true;
                try
                {
                    _vacantJobs = await VacantJobService.GetAllAsync(_tokenSource.Token);
                    _jobAdverts = await JobAdvertService.GetAllAsync(_tokenSource.Token);

                    foreach(var vacantJob in _vacantJobs)
                    {
                        foreach(var advert in _jobAdverts)
                        {
                            if (vacantJob.Id == advert.Id)
                            {
                                var tuple = (
                                    id: vacantJob.Id, 
                                    title: advert.Title, 
                                    name: vacantJob.Company.Name, 
                                    summary: advert.Summary, 
                                    RegDateTime: advert.RegistrationDateTime,
                                    url: vacantJob.URL, 
                                    categoryid: advert.Category.Id);

                                cards.Add(tuple);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                _isLoadingData = false;
                await base.OnAfterRenderAsync(firstRender);
            }
        }




    }
}
