using Microsoft.AspNetCore.Components;

namespace BlazorWebsite.Shared.Components.Diverse
{
    public partial class MessageAlert : ComponentBase
    {
        [Parameter] public string Message { get; set; } = string.Empty;
        [Parameter] public string MessageOptional { get; set; } = string.Empty;
        [Parameter] public AlertType Alert { get; set; }
        [Parameter] public bool IsLoading { get; set; } = false;
        [Parameter] public bool FullWidth { get; set; } = true;

        public string Container
        {
            get
            {
                if (FullWidth)
                    return "container-fluid";
                else
                    return "container";
            }
        }

        protected override void OnInitialized()
        {
            StateHasChanged();
        }

        public enum AlertType { Info, Warning, Error, Success }
    }
}
