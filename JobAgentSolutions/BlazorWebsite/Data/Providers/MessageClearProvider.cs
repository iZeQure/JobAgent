using System;

namespace BlazorWebsite.Data.Providers
{
    public class MessageClearProvider
    {
        public event EventHandler<bool> MessageCleared;
    }
}
