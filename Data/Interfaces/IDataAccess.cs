using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Interfaces
{
    interface IDataAccess : IDisposable
    {
        string GetConnectionString();
        void OpenConnection();
        void CloseConnection();
    }
}
