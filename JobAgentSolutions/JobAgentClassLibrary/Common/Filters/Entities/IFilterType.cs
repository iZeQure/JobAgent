using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IFilterType : IFilter
    {
        public string Name { get; }
        public string Description { get; }
    }
}
