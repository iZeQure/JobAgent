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
        private int cvr;
        private string name;
        private string url;
        #endregion

        #region Properties
        public int CVR { get { return cvr; } set { cvr = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string URL { get { return url; } set { url = value; } }
        #endregion
    }
}
