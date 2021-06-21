using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of Specialization.
    /// </summary>
    public class Specialization : BaseEntity
    {
        private Category _category;
        private string _name;

        public Specialization(int id, Category category, string name) : base (id)
        {
            _category = category;
            _name = name;
        }

        /// <summary>
        /// Reresents the associated <see cref="Category"/> for the specialization.
        /// </summary>
        public Category Category { get { return _category; } set { _category = value; } }

        /// <summary>
        /// The name of the specialization.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
    }
}
