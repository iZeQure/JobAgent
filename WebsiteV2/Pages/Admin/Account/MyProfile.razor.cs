using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Data.Services.Abstractions;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components.Web;
using ObjectLibrary.Common;
using System.Threading;

namespace BlazorServerWebsite.Pages.Admin.Account
{
    public partial class MyProfile : ComponentBase
    {
        private EditContext _editContext;
        private readonly AccountProfileModel _accountProfileModel = new();
        [Inject] private MyAuthStateProvider MyAuthStateProvider { get; set; }
        ////[Inject] private IDataService DataService { get; set; }
        //[Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IUserService UserService { get; set; }

        private string ErrorMessage { get; set; }
        private string Message { get; set; }
        private string UserEmail { get; set; }

        private bool UserAlreadyExists = false;

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

        private async Task OnFocusOut_CheckUserAlreadyExists(FocusEventArgs e)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            if (UserService is IUserService service)
            {
                if (UserEmail != _accountProfileModel.Email)
                {
                    try
                    {
                        UserAlreadyExists = await service.ValidateUserExistsByEmail(_accountProfileModel.Email, token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }

                }
            }
            
        }

    }
}
