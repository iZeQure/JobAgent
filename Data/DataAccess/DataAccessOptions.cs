using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.DataAccess
{
    public class DataAccessOptions
    {
        public static string ConnectionString { get; set; }

        public const string DataAccess = "DataAccessOptions";

        public string LogDB { get; set; }
        public string JobAgentDB { get; set; }
        public string DevDB { get; set; }
        public string HomeDB { get; set; }
    }
}
