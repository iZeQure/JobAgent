using BlazorWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Diverse
{
    public partial class MessageAlert : ComponentBase
    {
        [Parameter] public string Message { get; set; }
        [Parameter] public string MessageOptional { get; set; } = string.Empty;
        [Parameter] public AlertType Alert { get; set; }
        [Parameter] public bool IsLoading { get; set; }
        [Parameter] public bool FullWidth { get; set; }
        [Inject] protected ILogger<MessageAlert> Logger { get; set; }

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

        protected override Task OnParametersSetAsync()
        {
            if (string.IsNullOrEmpty(Message))
            {
                return Task.CompletedTask;
            }

            var x = Task.Delay(3000)
                .ContinueWith(x =>
                {
                    Logger.LogInformation("Trying to clear: {0}", Message);
                    Message = "";
                    return x.IsCompleted;
                });

            return x;
        }

        public enum AlertType { Info, Warning, Error, Success }


    }
}
