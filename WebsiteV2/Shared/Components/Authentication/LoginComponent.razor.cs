using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV2.Data.Models;

namespace WebsiteV2.Shared.Components.Authentication
{
    public partial class LoginComponent : ComponentBase
    {
        private EditContext _authContext;
        private readonly AuthenticationModel _authenticationModel = new();
        private bool _isLogInDisabled = true;

        public string ValidationMessage { get; set; }

        protected override Task OnInitializedAsync()
        {
            _authContext = new EditContext(_authenticationModel);
            _authContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
        }        

        private Task OnValidSubmit_LogInAsync()
        {
            ValidationMessage = "Logger ind, vent venligst..";

            return ClearValidationMessageAfterInterval();
        }

        private Task OnInvalidSubmit_DisplayError()
        {
            ValidationMessage = "Der er fejl i formen, tjek venligst.";

            return ClearValidationMessageAfterInterval();
        }

        private Task ClearValidationMessageAfterInterval(int milliseconds = 3000)
        {
            var x = Task.Delay(3000)
                .ContinueWith(x =>
                {
                    ValidationMessage = string.Empty;
                    return x.IsCompleted;
                });

            return x;
        }
    }
}
