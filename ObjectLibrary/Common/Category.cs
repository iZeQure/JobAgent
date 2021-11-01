using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information about Category.
    /// </summary>
    public class Category : BaseEntity<int>
    {
        private string _name;
        private List<Specialization> _specializations = new List<Specialization>();

        public Category(int id, string name) : base (id)
        {
            _name = name;
        }

        /// <summary>
        /// Defines the name of the category.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
        public List<Specialization> Specializations { get { return _specializations; }}



        public void AssignSpecialization(Specialization specialization) 
        {
            _specializations.Add(specialization);
        }

    }
}
