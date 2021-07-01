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

        private int chosenCategoryId;
        private List<(int, string, string, string, DateTime, string, int)> cards = new List<(int, string, string, string, DateTime, string, int)>();
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            _tokenSource = new();
            _vacantJobModel = new(); 

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
                catch (Exception)
                {
                    throw;
                }

                _isLoadingData = false;
                await base.OnAfterRenderAsync(firstRender);
            }
        }

    }
}
