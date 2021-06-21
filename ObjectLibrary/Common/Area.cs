using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of an Area entity.
    /// </summary>
    public class Area : BaseEntity
    {
        private string _name;

        public Area(int id, string name) : base (id)
        {
            _name = name;
        }

        /// <summary>
        /// Get the name of the Area.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
    }
}
