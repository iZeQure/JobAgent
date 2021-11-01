using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters.Entities
{
    public interface IFilter : IAggregateRoot, IEntity<int>
    {
    }
}
