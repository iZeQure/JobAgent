using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CrawlerLogModals
{
    public partial class EditCrawlerLogModal : ComponentBase
    {
        [Parameter] public LogModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private List<LogSeverity> _logSeverities = new();
        private List<LogType> _logTypes = new();
        private IEnumerable<ILog> _logs;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {

            foreach (LogSeverity item in Enum.GetValues(typeof(LogSeverity)))
            {
                _logSeverities.Add(item);
            }

            foreach (LogType type in Enum.GetValues(typeof(LogType)))
            {
                _logTypes.Add(type);
            }

            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var logTask = LogService.GetAllSystemLogsAsync();

                await Task.WhenAll(logTask);

                _logs = logTask.Result;
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

        private async Task OnValidSubmit_EditLogAsync()
        {
            if (Model.IsProcessing is true)
            {
                return;
            }
            using (var _ = Model.TimedEndOfOperation())
            {
                SystemLog DbLog = new()
                {
                    Id = Model.Id,
                    Action = Model.Action,
                    Message = Model.Message,
                    LogSeverity = Model.LogSeverity,
                    CreatedBy = Model.CreatedBy,
                    CreatedDateTime = Model.CreatedDateTime,
                    LogType = Model.LogType
                };

                var result = await LogService.UpdateAsync(DbLog);

                if (result is null)
                {
                    _errorMessage = "Fejl under opdatering af Log.";
                    return;
                }
            }

            if (Model.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditCrawlerLog");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}"); 
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new LogModel();
            StateHasChanged();
        }
    }
}
