using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class JobAdvertCategorySpecialization : BaseEntity
    {
        #region Attributes
        private string name;
        private string description;
        private JobAdvertCategory jobAdvertCategoryId;
        #endregion

        #region Properties
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public JobAdvertCategory JobAdvertCategoryId { get { return jobAdvertCategoryId; } set { jobAdvertCategoryId = value; } }
        #endregion
    }
}
