using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
{
    public class Company : BaseEntity
    {
        #region Attributes
        private string name;
        private string url;
        #endregion

        #region Properties
        public string Name { get { return name; } set { name = value; } }
        public string URL { get { return url; } set { url = value; } }
        #endregion
    }
}
