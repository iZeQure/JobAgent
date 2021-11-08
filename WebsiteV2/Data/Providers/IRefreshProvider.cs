using System;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Providers
{
    public interface IRefreshProvider
    {
        event Func<Task> RefreshRequest;

        void CallRefreshRequest();
    }
}