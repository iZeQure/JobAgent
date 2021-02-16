using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services.Interfaces
{
    public interface IRefresh
    {
        event Func<Task> RefreshRequest;

        void CallRefreshRequest();
    }
}
