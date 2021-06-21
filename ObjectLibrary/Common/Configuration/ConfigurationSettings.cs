using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common.Configuration
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        public string ConnectionString { get; set; }

        public string JwtSecurityKey { get; set; }
    }
}
