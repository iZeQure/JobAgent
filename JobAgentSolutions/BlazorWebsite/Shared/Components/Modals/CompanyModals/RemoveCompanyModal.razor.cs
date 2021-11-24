using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class RemoveCompanyModal : ComponentBase
    {
        [Parameter] public int CompanyId { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private string _errorMessage = "";
        private bool _isProcessing = false;

        private async Task OnClick_RemoveCompany(int id)
        {
            try
            {
                _isProcessing = true;

                Company company = new()
                {
                    Id = CompanyId
                };

                var result = await CompanyService.RemoveAsync(company);

                if (!result)
                {
                    _errorMessage = "Kunne ikke fjerne virksomheden, virksomheden er muligvis allerede slettet.";

                    return;
                }


                RefreshProvider.CallRefreshRequest();
                await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveCompany");
            }
            catch (Exception ex)
            {
                _errorMessage = "Kunne ikke fjerne virksomheden grundet ukent fejl.";
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private void CancelRequest(MouseEventArgs e)
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
