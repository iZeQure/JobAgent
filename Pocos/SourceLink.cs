using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pocos
{
    public class SourceLink : BaseEntity
    {
        #region Fields
        private Company company;
        private string link;
        #endregion

        #region Properties
        public Company Company { get { return company; } set { company = value; } }
        public string Link { get { return link; } set { link = value; } } 
        #endregion
    }
}