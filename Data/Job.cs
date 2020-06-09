using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class Job
    {
        #region Attributes
        private string jobTitle;
        private List<JobCategory> jobCategories;
        #endregion

        #region Properites
        public string JobTitle { get { return jobTitle; } set { jobTitle = value; } }

        public List<JobCategory> JobCategories { get { return jobCategories; } set { jobCategories = value; } }
        #endregion
    }
}
