using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Extensions;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using JobAgentClassLibrary.Security.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CrawlerLogModals
{
    public partial class CreateCrawlerLogModal : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private LogModel _logModel = new();
        private IEnumerable<ILog> _logs;
        private List<LogSeverity> _logSeverities = new();
        private List<LogType> _logTypes = new();
        private EditContext _editContext;

        private string _errorMessage = "";
        private bool _isLoading = false;
        private string _sessionUserEmail;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_logModel);
            _editContext.AddDataAnnotationsValidation();

            foreach (LogSeverity severity in Enum.GetValues(typeof(LogSeverity)))
            {
                _logSeverities.Add(severity);
            }

            foreach (LogType type in Enum.GetValues(typeof(LogType)))
            {
                _logTypes.Add(type);
            }

            var session = await AuthState;

            foreach (var sessionClaim in session.User.Claims)
            {
                if (sessionClaim.Type == ClaimTypes.Email)
                {
                    _sessionUserEmail = sessionClaim.Value;
                }
            }

            await LoadModalInformation();
        }

        private async Task LoadModalInformation()
        {
            _isLoading = true;

            try
            {
                var logTask = LogService.GetAllDbLogsAsync();

                try
                {
                    await TaskExtProvider.WhenAll(logTask);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }

                _logs = logTask.Result;
            }
            catch (Exception)
            {
                _errorMessage = "Uventet fejl. Prøv at genindlæse siden.";
            }
            finally
            {
                _isLoading = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit_CreateLogAsync()
        {
            if (_logModel.IsProcessing is true)
            {
                return;
            }
            using (var _ = _logModel.TimedEndOfOperation())
            {
                DbLog DbLog = new()
                {
                    Id = _logModel.Id,
                    Action = _logModel.Action,
                    Message = _logModel.Message,
                    LogSeverity = _logModel.LogSeverity,
                    CreatedBy = _sessionUserEmail,
                    CreatedDateTime = DateTime.Now,
                    LogType = LogType.CRAWLER
                };

                var result = await LogService.CreateAsync(DbLog);

                if (result is null)
                {
                    _errorMessage = "Fejl under oprettelse af Log.";
                    return;
                }
            }

            if (_logModel.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateCrawlerLog");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_logModel.Id}");
            }
        }

        private void OnClick_CancelRequest()
        {
            _logModel = new();
            _editContext = new(_logModel);
            StateHasChanged();
        }
    }
}
