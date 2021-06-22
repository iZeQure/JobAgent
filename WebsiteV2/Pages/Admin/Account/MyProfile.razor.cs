using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class MyProfile : ComponentBase
    {
        private EditContext _editContext;
        private readonly AccountProfileModel _accountProfileModel = new();

        protected override Task OnInitializedAsync()
        {
            _editContext = new(_accountProfileModel);
            _editContext.AddDataAnnotationsValidation();

            return base.OnInitializedAsync();
        }

        private Task OnValidSubmit_ChangeUserInformation()
        {
            if (_accountProfileModel.AccountId == 0)
            {
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
