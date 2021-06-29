using BlazorServerWebsite.Data.FormModels;
using BlazorServerWebsite.Components.Notification;
using BlazorServerWebsite.Shared.Components.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.Providers;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class Company : ComponentBase
    {
        private EditContext _editContext;

        private readonly CompanyModel _companyModel = new();

        [Inject] protected CompanyService CompanyService { get; set; }

        [Inject] protected IJSRuntime JSRuntime { get; set; }

        [Inject] protected IRefreshProvider RefreshProvider { get; set; }

        private IEnumerable<Company> companies = new List<Company>();

        private int _companyId = 0;
        
        private CompanyModel model { get; set; } = new CompanyModel();
        private string errorMessage = string.Empty;
        private bool dataIsLoading = false;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += UpdateContentAsync;

            companies = await LoadData();

            dataIsLoading = false;
        }

        private async Task<IEnumerable<Company>> LoadData()
        {
            dataIsLoading = true;

            return await CompanyService.GetAllCompaniesAsync();
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                var companies = await CompanyService.GetAllCompaniesAsync();

                if (companies != null)
                {
                    this.companies = companies;
                    return;
                }

                this.companies = null;
            }
            catch (Exception) { errorMessage = "Ukendt Fejl ved opdatering af virksomheder."; }
            finally { StateHasChanged(); }
        }

        private async void OnClick_OpenEditModal(int id)
        {
            var company = await CompanyService.GetCompanyByIdAsync(id);

            model = new CompanyModel()
            {
                CompanyId = company.Identifier,
                CVR = company.CVR,
                Name = company.Name,
                ContactPerson = company.ContactPerson
            };
        }

        private void OnClick_RemoveCompanyModal(int id)
        {
            _companyId = id;
        }


    }
}
