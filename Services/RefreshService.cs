using JobAgent.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class RefreshService : IRefresh
    {
        public event Action RefreshRequest;

        public void CallRefreshRequest()
        {
            RefreshRequest?.Invoke();
        }
    }
}
