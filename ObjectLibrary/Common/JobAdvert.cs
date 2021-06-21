using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information about job advert.
    /// </summary>
    public class JobAdvert : BaseEntity
    {
        private Category _category;
        private Specialization _specialization;
        private string _title;
        private string _summaray;
        private DateTime _registrationDateTime;

        public JobAdvert(int vacantJobId, Category category, Specialization specialization, string title, string summary, DateTime registrationDateTime) : base(vacantJobId)
        {
            _title = title;
            _summaray = summary;
            _registrationDateTime = registrationDateTime;
            _category = category;
            _specialization = specialization;
        }

        /// <summary>
        /// Describes which category the job advert is associated with.
        /// </summary>
        public Category Category { get { return _category; } set { _category = value; } }

        /// <summary>
        /// Describes the specialization which the job advert is assoicated with.
        /// </summary>
        public Specialization Specialization { get { return _specialization; } set { _specialization = value; } }

        /// <summary>
        /// Contains the title of the created job advert.
        /// </summary>
        public string Title { get { return _title; } set { _title = value; } }

        /// <summary>
        /// Contains a summary of the job advert.
        /// </summary>
        public string Summary { get { return _summaray; } set { _summaray = value; } }

        /// <summary>
        /// Specifies the Date of which the job advert were created.
        /// </summary>
        public DateTime RegistrationDateTime { get { return _registrationDateTime; } set { _registrationDateTime = value; } }
    }
}
