using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.SystemLogModals
{
    public partial class CreateSystemLogModal : ComponentBase
    {
        [CascadingParameter] private Task<AuthenticationState> AuthState { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected ILogService LogService { get; set; }

        private LogModel _logModel = new();
        private readonly List<LogSeverity> _logSeverities = new();
        private readonly List<LogType> _logTypes = new();
        private EditContext _editContext;

        private string _errorMessage = "";
        private readonly bool _isLoading = false;
        private bool _isProcessing = false;
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
        }
        private async Task OnValidSubmit_CreateLogAsync()
        {
            _isProcessing = true;
            try
            {
                SystemLog SystemLog = new()
                {
                    Id = _logModel.Id,
                    Action = _logModel.Action,
                    Message = _logModel.Message,
                    LogSeverity = _logModel.LogSeverity,
                    CreatedBy = _sessionUserEmail,
                    CreatedDateTime = DateTime.Now,
                    LogType = LogType.DATABASE
                };

                bool isCreated = false;
                var result = await LogService.CreateAsync(SystemLog);

                if (result.Id == _logModel.Id && result.Message == _logModel.Message)
                {
                    isCreated = true;
                }

                if (!isCreated)
                {
                    _errorMessage = "Kunne ikke oprette Database Log grundet ukendt fejl";
                }

                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalCreateSystemLog");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{_logModel.Id}");

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
            _logModel = new();
            _editContext = new(_logModel);
            StateHasChanged();
        }
    }
}
