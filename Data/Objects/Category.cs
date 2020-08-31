using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
{
    public class Category : BaseEntity
    {
        #region Attributes
        private string name;
        private List<Specialization> specializations;
        #endregion

        #region Properties
        /// <summary>
        /// Defines the name of the Category.
        /// </summary>
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// A collection of specializations if the category has any associated.
        /// </summary>
        public List<Specialization> Specializations {  get { return specializations; }  set { specializations = value; } }
        #endregion
    }
}
