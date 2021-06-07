using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pocos
{
    public class Location : BaseEntity
    {
        #region Fields
        private string name;
        private string description;
        #endregion

        #region Properties
        /// <summary>
        /// The physical name of the Location.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }
        /// <summary>
        /// Provides the address, or null as a description for the Location, depends on the name.
        /// </summary>
        public string Description { get { return description; } set { description = value; } }
        #endregion
    }
}
