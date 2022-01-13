using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.UserAccessModals
{
    public partial class EditUserAccessModal : ComponentBase
    {
        [Parameter] public BasicUserModel Model { get; set; }
        [Parameter] public IEnumerable<IRole> Roles { get; set; }
        [Parameter] public IEnumerable<ILocation> Locations { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected IRoleService RoleService { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }

        private IEnumerable<IUser> _users;

        private string _errorMessage = "";
        private bool _isLoading = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadModalInformationAsync();
        }

        private async Task LoadModalInformationAsync()
        {
            _isLoading = true;

            try
            {
                var userTask = UserService.GetUsersAsync();

                await Task.WhenAll(userTask);

                _users = userTask.Result;


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

        private async Task OnValidSubmit_EditUser()
        {
            if (Model.IsProcessing is true)
            {
                return;
            }

            using (var _ = Model.TimedEndOfOperation())
            {
                IUser user = new User()
                {
                    Id = Model.Id,
                    RoleId = Model.RoleId,
                    LocationId = Model.LocationId,
                    FirstName = Model.FirstName,
                    LastName = Model.LastName,
                    Email = Model.Email
                };

                var result = await UserService.UpdateAsync(user);

                if (result is null)
                {
                    _errorMessage = "Fejl i opdatering af brugeren.";
                    return;
                }
            }

            if (Model.IsProcessing is false)
            {
                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalEditUserAccess");
                await JSRuntime.InvokeVoidAsync("onInformationChangeAnimateTableRow", $"{Model.Id}");
            }
        }

        private void OnClick_CancelRequest()
        {
            Model = new BasicUserModel();
            StateHasChanged();
        }
    }
}
