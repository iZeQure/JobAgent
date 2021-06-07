using JobAgent.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class RefreshService : IRefresh
    {
        public event Func<Task> RefreshRequest;

        public void CallRefreshRequest()
        {
            RefreshRequest?.Invoke();
        }
    }
}
