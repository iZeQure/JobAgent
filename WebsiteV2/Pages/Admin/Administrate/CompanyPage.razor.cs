using BlazorServerWebsite.Data.FormModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data.Providers;
using BlazorServerWebsite.Data.Services.Abstractions;
using System.Threading;
using ObjectLibrary.Common;

namespace BlazorServerWebsite.Pages.Admin.Administrate
{
    public partial class CompanyPage : ComponentBase
    {
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }

        private readonly CancellationTokenSource _tokenSource = new();
        private CompanyModel _companyModel = new();
        private IEnumerable<Company> _companies = new List<Company>();

        private int _companyId = 0;
        private string errorMessage = string.Empty;
        private bool _isLoadingData = false;

        protected override async Task OnInitializedAsync()
        {
            RefreshProvider.RefreshRequest += UpdateContentAsync;

            _companies = await CompanyService.GetAllAsync(_tokenSource.Token);

            await base.OnInitializedAsync();
        }

        private async Task UpdateContentAsync()
        {
            try
            {
                _companies = await CompanyService.GetAllAsync(_tokenSource.Token);
            }
            catch (Exception) { errorMessage = "Ukendt Fejl ved opdatering af virksomheder."; }
            finally { StateHasChanged(); }
        }

        private async void OnClick_OpenEditModal(int id)
        {
            try
            {
                var company = await CompanyService.GetByIdAsync(id, _tokenSource.Token);

                _companyModel = new CompanyModel()
                {
                    CompanyId = company.Id,
                    CVR = company.CVR,
                    Name = company.Name,
                    ContactPerson = company.ContactPerson
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
