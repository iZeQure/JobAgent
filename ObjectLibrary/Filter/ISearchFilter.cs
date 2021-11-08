using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Filter
{
    public interface ISearchFilter
    {
        /// <summary>
        /// Get key of the filter.
        /// </summary>
        string GetKey { get; }
    }
}
