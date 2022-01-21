using BlazorWebsite.Data.Providers;
using Microsoft.AspNetCore.Components;
using System;
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
        [Inject] protected MessageClearProvider MessageClearProvider { get; set; }

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
                    if (string.IsNullOrEmpty(Message))
                    {
                        return x.IsCompleted;
                    }
                    Message = "";
                    OnMessageCleared(true);
                    return x.IsCompleted;
                });

            return x;
        }
        public event EventHandler<bool> MessageCleared;
        protected virtual void OnMessageCleared(bool result)
        {
            EventHandler<bool> handler = MessageCleared;
            if (handler is not null)
            {
                handler(this, result);
            }
        }

        public enum AlertType { Info, Warning, Error, Success }


    }
}
