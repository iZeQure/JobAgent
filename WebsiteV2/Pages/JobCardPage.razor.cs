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
        [Inject] protected IVacantJobService VacantJobService { get; set; }
        [Inject] protected IJobAdvertService JobAdvertService { get; set; }

        private CancellationTokenSource _tokenSource;
        private IEnumerable<VacantJob> _vacantJobs;
        private IEnumerable<JobAdvert> _jobAdverts;
        private VacantJobModel _vacantJobModel;

        private List<string> cards = new List<string>();
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            _vacantJobModel = new(); 

            await LoadInformation();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
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
                                cards.Add(vacantJob.Id.ToString());
                                cards.Add(advert.Title);
                                cards.Add(vacantJob.Company.Name);
                                cards.Add(advert.Summary);
                                cards.Add(advert.RegistrationDateTime.ToString());
                                cards.Add(vacantJob.URL);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                _isLoadingData = false;
                await base.OnAfterRenderAsync(firstRender);
            }
        }

        private async Task LoadInformation()
        {
            await LoadInformation();
        }

        
    }
}
