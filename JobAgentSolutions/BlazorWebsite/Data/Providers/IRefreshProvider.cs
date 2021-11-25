using System;
using System.Threading.Tasks;

namespace BlazorWebsite.Data.Providers
{
    public interface IRefreshProvider
    {
        event Func<Task> RefreshRequest;

        void CallRefreshRequest();
    }
}
