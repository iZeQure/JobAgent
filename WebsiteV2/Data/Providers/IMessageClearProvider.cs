using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Providers
{
    public interface IMessageClearProvider
    {
        /// <summary>
        /// Clears a message after a specific time.
        /// </summary>
        /// <param name="message">A message containing the string to clear.</param>
        /// <param name="milliseconds">Amount of time before the clear should happen in milliseconds.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task ClearMessageOnIntervalAsync(string message, int milliseconds = 3000);
    }
}
