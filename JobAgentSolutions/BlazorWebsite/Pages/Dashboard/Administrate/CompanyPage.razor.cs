using BlazorWebsite.Data.FormModels;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class CompanyPage : ComponentBase
    {
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }

        private CompanyModel _companyModel = new();
        private IEnumerable<ICompany> _companies = new List<Company>();

        private int _companyId = 0;
        private string errorMessage = string.Empty;
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += UpdateContentAsync;

            _companies = await CompanyService.GetAllAsync();

            await base.OnInitializedAsync();
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                _companies = await CompanyService.GetAllAsync();
            }
            catch (Exception) { errorMessage = "Ukendt Fejl ved opdatering af virksomheder."; }
            finally { StateHasChanged(); }
        }

        private async void OnClick_OpenEditModal(int id)
        {
            try
            {
                var company = await CompanyService.GetByIdAsync(id);

                _companyModel = new CompanyModel()
                {
                    CompanyId = company.Id,
                    Name = company.Name,
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

        private void OnClick_RemoveCompanyModal(int id)
        {
            _companyId = id;
        }
    }
}
