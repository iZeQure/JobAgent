using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.DbLogModals
{
    public partial class EditDbLogModal : ComponentBase
    {
        [Parameter] public LogModel Model { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private List<LogSeverity> _logSeverities = new();
        private List<LogType> _logTypes = new();
        private IEnumerable<ILog> _logs;

        private string _errorMessage = "";
        private bool _isProcessing = false;
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
                var logTask = LogService.GetAllDbLogsAsync();

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

        private async Task OnValidSubmit_EditJobVacancy()
        {
            _isProcessing = true;

            try
            {
                DbLog DbLog = new()
                {
                    Id = Model.Id,
                    Action = Model.Action,
                    Message = Model.Message,
                    LogSeverity = Model.LogSeverity,
                    CreatedBy = Model.CreatedBy,
                    CreatedDateTime = Model.CreatedDateTime,
                    LogType = LogType.DATABASE
                };

                bool isUpdated = false;
                var result = await LogService.UpdateAsync(DbLog);

                if (result.Id == Model.Id && result.Message == Model.Message)
                {
                    isUpdated = true;
                }

                if (!isUpdated)
                {
                    _errorMessage = "Kunne ikke opdatere Log.";
                    return;
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditDbLog");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new LogModel();
            StateHasChanged();
        }
    }
}
