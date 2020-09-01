using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgent.Data.Objects
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
        private string sourceUrl;
        private Company companyCVR;
        private Category category;
        private Specialization specialization;
        #endregion

        #region Properties
        public string JobVacancyRegisteredAgo
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                bool approximate = true;
                var value = JobRegisteredDate;

                string suffix = (value > DateTime.Now) ? " fra nu" : " siden";

                TimeSpan timeSpan = new TimeSpan(Math.Abs(DateTime.Now.Subtract(value).Ticks));

                if (timeSpan.Days > 0)
                {
                    sb.AppendFormat("{0} {1}", timeSpan.Days,
                      (timeSpan.Days > 1) ? "dage" : "dag");
                    if (approximate) return sb.ToString() + suffix;
                }
                if (timeSpan.Hours > 0)
                {
                    sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                      timeSpan.Hours, (timeSpan.Hours > 1) ? "timer" : "time");
                    if (approximate) return sb.ToString() + suffix;
                }
                if (timeSpan.Minutes > 0)
                {
                    sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                      timeSpan.Minutes, (timeSpan.Minutes > 1) ? "minutter" : "minut");
                    if (approximate) return sb.ToString() + suffix;
                }
                if (timeSpan.Seconds > 0)
                {
                    sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
                      timeSpan.Seconds, (timeSpan.Seconds > 1) ? "sekunder" : "sekund");
                    if (approximate) return sb.ToString() + suffix;
                }
                if (sb.Length == 0) return "lige nu";

                sb.Append(suffix);
                return sb.ToString();
            }
        }

        public string Title { get { return title; } set { title = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string PhoneNumber { get { return phoneNumber; } set { phoneNumber = value; } }
        public string JobDescription { get { return jobDescription; } set { jobDescription = value; } }
        public string JobLocation { get { return jobLocation; } set { jobLocation = value; } }
        public DateTime JobRegisteredDate { get { return jobRegisteredDate; } set { jobRegisteredDate = value; } }
        public DateTime DeadlineDate { get { return deadlineDate; } set { deadlineDate = value; } }
        public string SourceURL { get { return sourceUrl; } set { sourceUrl = value; } }
        public Company CompanyCVR { get { return companyCVR; } set { companyCVR = value; } }
        public Category Category { get { return category; } set { category = value; } }
        public Specialization Specialization { get { return specialization; } set { specialization = value; } }
        #endregion
    }
}
