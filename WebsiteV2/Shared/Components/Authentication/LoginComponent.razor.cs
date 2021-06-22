using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;

namespace BlazorServerWebsite.Shared.Components.Authentication
{
    public partial class LoginComponent : ComponentBase
    {
        private EditContext _editContext;
        private bool _processingRequest;
        private readonly AuthenticationModel _authenticationModel = new();

        public string ValidationMessage { get; set; }

        public bool ProcessingRequest { get { return _processingRequest; } }

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_authenticationModel);
            _editContext.AddDataAnnotationsValidation();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _editContext.OnFieldChanged += LoginForm_OnFieldChanged;
                Console.WriteLine($"Rendered Login Component");
            }

            return base.OnAfterRenderAsync(firstRender);
        }

        private void LoginForm_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            Console.WriteLine($"{e.FieldIdentifier.FieldName} has been changed.");

            ValidationMessage = $"{e.FieldIdentifier.FieldName} has been changed.";
            ClearValidationMessageAfterInterval();
        }

        private Task OnValidSubmit_LogInAsync()
        {
            ValidationMessage = "Logger ind, vent venligst..";

            return ClearValidationMessageAfterInterval();
        }

        private Task ClearValidationMessageAfterInterval(int milliseconds = 3000)
        {
            var x = Task.Delay(milliseconds)
                .ContinueWith(x =>
                {
                    ValidationMessage = string.Empty;
                    return x.IsCompleted;
                });

            return x;
        }
    }
}
