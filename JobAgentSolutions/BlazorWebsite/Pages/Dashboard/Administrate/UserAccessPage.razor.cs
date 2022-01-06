using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class UserAccessPage : ComponentBase
    {
        [Inject] private IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IUserService UserService { get; set; }
        [Inject] protected IRoleService RoleService { get; set; }
        [Inject] protected ILocationService LocationService { get; set; }

        private BasicUserModel _userModel = new();
        private IEnumerable<IUser> _users;
        private IEnumerable<IRole> _roles;
        private IEnumerable<ILocation> _locations;
        private IUser _user;
        private int _userId;

        private int _jobPageId = 0;
        private bool dataIsLoading = true;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += RefreshContent;

            await LoadData();
        }

        private async Task LoadData()
        {
            dataIsLoading = true;
            try
            {
                var userTask = UserService.GetUsersAsync();
                var roleTask = RoleService.GetRolesAsync();
                var locationTask = LocationService.GetLocationsAsync();

                await Task.WhenAll(userTask, roleTask, locationTask);

                _users = userTask.Result;
                _roles = roleTask.Result;
                _locations = locationTask.Result;
            }
            finally
            {
                dataIsLoading = false;
                StateHasChanged();
            }
        }

        private void ConfirmationWindow(int id)
        {
            _userId = id;
        }

        private async Task OnClick_EditLink(int id)
        {
            try
            {
                _user = await UserService.GetUserByIdAsync(id);

                _userModel = new BasicUserModel()
                {
                    Id = _user.Id,
                    RoleId = _user.RoleId,
                    LocationId = _user.LocationId,
                    FirstName = _user.FirstName,
                    LastName = _user.LastName,
                    Email = _user.Email
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open EditModal error: {ex.Message}");
            }
            finally
            {
                StateHasChanged();
            }
        }

        private async Task RefreshContent()
        {
            try
            {
                var links = await UserService.GetUsersAsync();

                if (links == null)
                {
                    return;
                }

                _users = links;
            }
            finally
            {
                StateHasChanged();
            }
        }
    }
}

