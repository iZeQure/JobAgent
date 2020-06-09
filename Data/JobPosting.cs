using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class JobPosting
    {
        #region Attributes
        private int jobPostingId;
        private string jobPostingTitle;
        private string jobPostingCompany;
        private string jobPostingImage;
        private string jobPostingInformation;
        private string jobPostingCategory;
        private int jobPostingAmountOfStudentsAvailable;
        private DateTime jobPostingPosted;
        #endregion

        #region Properites
        public int Id { get { return jobPostingId; } set { jobPostingId = value; } }
        public string Title { get { return jobPostingTitle; } set { jobPostingTitle = value; } }

        public string Company { get { return jobPostingCompany; } set { jobPostingCompany = value; } }

        public string ImageURL { get { return jobPostingImage; } set { jobPostingImage = value; } }

        public string Information { get { return jobPostingInformation; } set { jobPostingInformation = value; } }

        public string Catetory { get { return jobPostingCategory; } set { jobPostingCategory = value; } }

        public int StudentsNeeded { get { return jobPostingAmountOfStudentsAvailable; } set { jobPostingAmountOfStudentsAvailable = value; } }

        public DateTime DateTimeForPost { get { return jobPostingPosted; } set { jobPostingPosted = value; } }
        #endregion
    }
}
