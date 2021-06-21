using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information about the Vacant Jobs.
    /// </summary>
    public class VacantJob : BaseEntity
    {
        private Company _company;
        private string _url;

        public VacantJob(int id, Company company, string url) : base(id)
        {
            _company = company;
            _url = url;
        }

        /// <summary>
        /// Company that owns the specified job page.
        /// </summary>
        public Company Company { get { return _company; } set { _company = value; } }

        /// <summary>
        /// Is the url of the page that represents the job page.
        /// </summary>
        public string URL { get { return _url; } set { _url = value; } }
    }
}
