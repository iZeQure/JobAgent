using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Filter
{
    /// <summary>
    /// Represents a class storing the information of a filter type.
    /// </summary>
    public class FilterType : BaseEntity
    {
        private readonly string _name;
        private readonly string _description;

        public FilterType(int id, string name, string description) : base (id)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Gets the name of the filter.
        /// </summary>
        public string Name { get { return _name; } }

        /// <summary>
        /// Gets the description of the filter, if none returns <see cref="string.Empty"/>.
        /// </summary>
        public string Description { get { return _description; } }
    }
}
