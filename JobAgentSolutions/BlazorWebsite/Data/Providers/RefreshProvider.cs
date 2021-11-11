using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Data.Providers
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
