using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.SystemSettings
{
    public partial class CrawlerLogPage
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private LogModel _logModel = new();
        public IEnumerable<ILog> _logs = new List<DbLog>();

        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var logTask = LogService.GetAllAsync();

                await Task.WhenAll(logTask);

                _logs = logTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }
    }
}
