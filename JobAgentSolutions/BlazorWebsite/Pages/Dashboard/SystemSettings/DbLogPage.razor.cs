using BlazorWebsite.Data.FormModels;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.SystemSettings
{
    public partial class DbLogPage
    {
        [Inject] protected ILogService LogService { get; set; }

        private LogModel _logModel = new();
        private ILog _log;
        public IEnumerable<ILog> _logs = new List<DbLog>();
        private int _logId;

        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshContent;

            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var logTask = LogService.GetAllDbLogsAsync();

                await Task.WhenAll(logTask);

                _logs = logTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _logId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _log = await LogService.GetByIdAsync(id);

                _logModel = new LogModel
                {
                    Id = _log.Id,
                    Action = _log.Action,
                    Message = _log.Message,
                    LogSeverity = _log.LogSeverity,
                    CreatedBy = _log.CreatedBy,
                    CreatedDateTime = _log.CreatedDateTime,
                    LogType = LogType.DATABASE
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        public async override Task RefreshContent()
        {
            try
            {
                var logs = await LogService.GetAllDbLogsAsync();

                if (logs == null)
                {
                    return;
                }

                _logs = logs;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}
