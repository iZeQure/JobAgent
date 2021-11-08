using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of Location.
    /// </summary>
    public class Location : BaseEntity<int>
    {
        private string _name;

        public Location(int id, string name) : base (id)
        {
            _name = name;
        }

        /// <summary>
        /// Get the name of the location.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }
    }
}
