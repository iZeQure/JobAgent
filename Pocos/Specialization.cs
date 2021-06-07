using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pocos
{
    public class Specialization : BaseEntity
    {
        #region Fields
        private string name;
        private int categoryId;
        #endregion

        #region Properties
        /// <summary>
        /// Defines the name of the spcialization.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// The associated category id, that the specialization belongs to.
        /// </summary>
        public int CategoryId { get { return categoryId; } set { categoryId = value; } }
        #endregion
    }
}
