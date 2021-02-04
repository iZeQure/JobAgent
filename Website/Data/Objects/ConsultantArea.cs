using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
{
    public class ConsultantArea : BaseEntity
    {
        #region Attributes
        private string name;
        #endregion

        #region Properties
        /// <summary>
        /// Defines the actual name of the Consultant Area.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        #endregion
    }
}
