using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Filter
{
    /// <summary>
    /// Handles the information about <see cref="StaticSearchFilter"/>.
    /// </summary>
    public class StaticSearchFilter : BaseEntity, ISearchFilter
    {
        private FilterType _filterType;
        private string _key;

        public StaticSearchFilter(int id, FilterType filterType, string key) : base (id)
        {
            _filterType = filterType;
            _key = key;
        }

        /// <summary>
        /// Get the type of the filter used.
        /// </summary>
        public FilterType FilterType { get { return _filterType; } }

        /// <summary>
        /// Get the value of the key for the specified type.
        /// </summary>
        public string GetKey { get { return _key; } }
    }
}
