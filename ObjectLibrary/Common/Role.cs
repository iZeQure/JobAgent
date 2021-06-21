using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of a role.
    /// </summary>
    public class Role : BaseEntity
    {
        private string _name;
        private string _description;

        public Role(int id, string name, string description = "") : base (id)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Get the name of the role.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// Get the description of the role, if no description associated it returns an empty string.
        /// </summary>
        public string Description { get { return _description; } set { _description = value; } }
    }
}
