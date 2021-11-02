using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas.Entities
{
    public class Area : IArea
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
