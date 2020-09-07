using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
{
    public class SourceLink : BaseEntity
    {
        private string link;

        public string Link { get { return link; } set { link = value; } }
    }
}