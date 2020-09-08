using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
{
    public class SourceLink : BaseEntity
    {
        private Company company;
        private string link;

        public Company Company { get { return company; } set { company = value; } }
        public string Link { get { return link; } set { link = value; } }
    }
}