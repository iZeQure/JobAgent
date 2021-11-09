using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Providers
{
    public class MessageClearProvider : IMessageClearProvider
    {
        public Task ClearMessageOnIntervalAsync(string message, int milliseconds = 3000)
        {
            var x = Task.Delay(milliseconds)
                .ContinueWith(x =>
                {
                    message = string.Empty;
                    return x.IsCompleted;
                });

            return x;
        }
    }
}
