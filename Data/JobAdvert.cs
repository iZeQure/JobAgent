using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class JobAdvert : BaseEntity
    {
        #region Attributes
        private string title;
        private string email;
        private string phoneNumber;
        private string jobDescription;
        private string jobLocation;
        private DateTime jobRegisteredDate;
        private DateTime deadlineDate;
        private Company companyCVR;
        private JobAdvertCategory jobAdvertCategoryId;
        private JobAdvertCategorySpecialization jobAdvertCategorySpecializationId;
        #endregion

        #region Properties
        public string Title { get { return title; } set { title = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        public string JobDescription { get { return jobDescription; } set { jobDescription = value; } }
        public string JobLocation { get { return jobLocation; } set { jobLocation = value; } }
        public DateTime JobRegisteredDate { get { return jobRegisteredDate; } set { jobRegisteredDate = value; } }
        public DateTime DeadlineDate { get { return deadlineDate; } set { deadlineDate = value; } }
        public Company CompanyCVR { get { return companyCVR; } set { companyCVR = value; } }
        public JobAdvertCategory JobAdvertCategoryId { get { return jobAdvertCategoryId; } set { jobAdvertCategoryId = value; } } 
        public JobAdvertCategorySpecialization JobAdvertCategorySpecializationId { get { return jobAdvertCategorySpecializationId; } set { jobAdvertCategorySpecializationId = value; } }
        #endregion
    }
}
