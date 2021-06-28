using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class Company : ComponentBase
    {
        //[Inject] protected DataService DataService { get; set; }
        //[Inject] protected IJSRuntime JSRuntime { get; set; }
        //[Inject] protected IRefresh RefreshService { get; set; }

        private IEnumerable<Company> companies = new List<Company>();

        private CompanyModel model { get; set; } = new CompanyModel();
        private int companyId { get; set; }
        private string errorMessage = string.Empty;
        private bool dataIsLoading = false;

        protected override async Task OnInitializedAsync()
        {
            RefreshService.RefreshRequest += UpdateContentAsync;

            companies = await LoadData();

            dataIsLoading = false;
        }

        private async Task<IEnumerable<Company>> LoadData()
        {
            dataIsLoading = true;

            return await DataService.GetAllCompaniesAsync();
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                var companies = await DataService.GetAllCompaniesAsync();

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
            var company = await DataService.GetCompanyByIdAsync(id);

            model = new CompanyModel()
            {
                Id = company.Identifier,
                CVR = company.CVR,
                Name = company.Name,
                URL = company.URL
            };
        }

        private void OnClick_RemoveCompanyModal(int id)
        {
            companyId = id;
        }


    }
}
