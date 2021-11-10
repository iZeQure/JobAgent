using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.CompanyModals
{
    public partial class RemoveCompanyModal : ComponentBase
    {
        [Parameter] public int CompanyId { get; set; }
        [Inject] protected IRefreshProvider RefreshProvider { get; set; }
        [Inject] protected ICompanyService CompanyService { get; set; }
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        private bool _isProcessing = false;

        private async Task OnClick_RemoveCompany(int id)
        {
            _isProcessing = true;

            Company company = new()
            {
                Id = CompanyId
            };
                  

            await CompanyService.RemoveAsync(company);

            _isProcessing = false;

            RefreshProvider.CallRefreshRequest();

            await JSRuntime.InvokeVoidAsync("toggleModalVisibility", "ModalRemoveCompany");
        }

        private void CancelRequest(MouseEventArgs e)
        {
            RefreshProvider.CallRefreshRequest();
        }
    }
}
