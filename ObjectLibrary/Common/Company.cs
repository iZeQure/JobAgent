using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of the company.
    /// </summary>
    public class Company : BaseEntity<int>
    {
        private int _cvr;
        private string _name;
        private string _contactPerson;

        public Company(int id, int cvr, string name, string contactPerson) : base (id)
        {
            _cvr = cvr;
            _name = name;
            _contactPerson = contactPerson;
        }

        /// <summary>
        /// Get the CVR number for the company.
        /// </summary>
        public int CVR { get { return _cvr; } set { _cvr = value; } }

        /// <summary>
        /// Get the name of the company.
        /// </summary>
        public string Name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// Get the contact person for the company.
        /// </summary>
        public string ContactPerson { get { return _contactPerson; } set { _contactPerson = value; } }
    }
}
