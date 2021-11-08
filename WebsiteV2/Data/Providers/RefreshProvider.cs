using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Providers
{
    public class RefreshProvider : IRefreshProvider
    {
        public event Func<Task> RefreshRequest;

        public void CallRefreshRequest()
        {
            RefreshRequest?.Invoke();
        }
    }
}
