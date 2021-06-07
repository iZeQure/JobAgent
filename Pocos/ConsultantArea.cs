using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pocos
{
    public class ConsultantArea : BaseEntity
    {
        #region Fields
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
