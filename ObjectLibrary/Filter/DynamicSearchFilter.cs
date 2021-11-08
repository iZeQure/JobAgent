using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Filter
{
    /// <summary>
    /// Handles the information for the <see cref="DynamicSearchFilter"/>.
    /// </summary>
    public class DynamicSearchFilter : BaseEntity, ISearchFilter
    {
        private readonly Category _category;
        private readonly Specialization _specialization;
        private readonly string _key;

        public DynamicSearchFilter(int id, Category category, Specialization specialization, string key) : base (id)
        {
            _category = category;
            _specialization = specialization;
            _key = key;
        }

        /// <summary>
        /// Gets the <see cref="Category"/> associated with the search filter.
        /// </summary>
        public Category GetCategory { get { return _category; } }

        /// <summary>
        /// Gets the <see cref="Specialization"/> associated with the search filter.
        /// </summary>
        public Specialization GetSpecialization { get { return _specialization; } }

        /// <summary>
        /// Get the value of the key for the specified type.
        /// </summary>
        public string GetKey { get { return _key; } }
    }
}
