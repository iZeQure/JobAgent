using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Interfaces
{
    public interface IRefresh
    {
        event Action RefreshRequest;

        void CallRefreshRequest();
    }
}
