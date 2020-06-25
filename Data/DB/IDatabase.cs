using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.DB
{
    interface IDatabase
    {
        void OpenConnection();
        void CloseConnection();
    }
}
