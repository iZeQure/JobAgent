using BlazorWebsite.Data.FormModels;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorWebsite.Pages.Dashboard.Administrate
{
    public partial class CompanyPage
    {
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private CompanyModel _companyModel = new();
        private IEnumerable<ICompany> _companies = new List<Company>();

        private int _companyId = 0;
        private string errorMessage = string.Empty;
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            _isLoadingData = true;
            try
            {
                _companies = await CompanyService.GetAllAsync();
            }
            finally
            {
                _isLoadingData = false;
                StateHasChanged();
            }
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

        public override async Task RefreshContent()
        {
            try
            {
                _companies = await CompanyService.GetAllAsync();
            }
            catch (Exception) { errorMessage = "Ukendt Fejl ved opdatering af virksomheder."; }
            finally { StateHasChanged(); }
        }

    }
}
