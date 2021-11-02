using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Core.Settings
{
    public class AppSecuritySettings : ISecuritySettings
    {
        public string JwtSecurityKey { get; set; }
    }
}
