using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV2.Data.Models;

namespace WebsiteV2.Shared
{
    public partial class NavbarMenu : ComponentBase
    {
        private EditContext _authContext;
        private readonly AuthenticationModel _authenticationModel = new();
        private string _userProfileState = string.Empty;
        private bool _isLogInDisabled = true;

        protected override Task OnInitializedAsync()
        {
            _authContext = new EditContext(_authenticationModel);
            _authContext.AddDataAnnotationsValidation();
            _authContext.OnFieldChanged += AuthContext_OnFieldChanged_Validate;

            return base.OnInitializedAsync();
        }

        private void AuthContext_OnFieldChanged_Validate(object sender, FieldChangedEventArgs e)
        {
            if (_authContext.Validate())
            {
                _isLogInDisabled = false;
                return;
            }

            _isLogInDisabled = true;
        }

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        private void OnClick_UserProfileIcon()
        {
            if (string.IsNullOrEmpty(_userProfileState))
            {
                _userProfileState = "active";
                return;
            }

            _userProfileState = string.Empty;
        }

        private Task OnValidSubmit_LogInAsync()
        {
            _isLogInDisabled = false;
            return Task.CompletedTask;
        }

        private void OnInvalidSubmit_DisableLogIn()
        {
            _isLogInDisabled = true;
        }
    }
}
