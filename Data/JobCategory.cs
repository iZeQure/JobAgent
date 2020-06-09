using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class JobCategory
    {
        #region Attributes
        private string[] menus;
        #endregion

        #region Properites
        public string[] Menus { get { return menus; } set { menus = value; } }
        #endregion
    }
}
