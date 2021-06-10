using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebsiteV2.Data.Models;

namespace WebsiteV2.Shared.Components.Authentication
{
    public partial class LoginAuth : ComponentBase
    {
        private EditContext _authContext;
        private readonly AuthenticationModel _authenticationModel = new();
        private string _userProfileState = string.Empty;

        protected override Task OnInitializedAsync()
        {
            _authContext = new EditContext(_authenticationModel);
            _authContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
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

        private Task OnSubmit_ValidateUserLogin()
        {
            return Task.CompletedTask;
        }
    }
}
