using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Versioning
{
    /// <summary>
    /// Handles the information about release types.
    /// </summary>
    public class ReleaseType : BaseEntity
    {
        private readonly string _name;

        public ReleaseType(int id, string name) : base (id)
        {
            _name = name;
        }

        /// <summary>
        /// Get the name of the release type.
        /// </summary>
        public string Name { get { return _name; } }
    }
}
