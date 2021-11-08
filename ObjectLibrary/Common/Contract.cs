using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of the contract.
    /// </summary>
    public class Contract : BaseEntity<int>
    {
        private Company _company;
        private IUser _user;
        private string _name;
        private DateTime _registrationDateTime;
        private DateTime _expiryDateTime;

        public Contract(Company company, IUser user, string name, DateTime registrationDateTime, DateTime expiryDateTime) : base(company.Id)
        {
            _company = company;
            _user = user;
            _name = name;
            _registrationDateTime = registrationDateTime;
            _expiryDateTime = expiryDateTime;
        }

        /// <summary>
        /// Get the Company for the Contract.
        /// </summary>
        public Company company { get { return _company; } set { _company = value; } }

        /// <summary>
        /// Get the user for the Contract.
        /// </summary>
        public IUser user { get { return _user; } set { _user = value; } }

        /// <summary>
        /// Get the Name for the Contract.
        /// </summary>
        public string name { get { return _name; } set { _name = value; } }

        /// <summary>
        /// Get the Registration Date for the Contract.
        /// </summary>
        public DateTime registrationDateTime { get { return _registrationDateTime; } set { _registrationDateTime = value; } }

        //// <summary>
        /// Get the Expiration Date for the Contract.
        /// </summary>
        public DateTime expiryDateTime { get { return _expiryDateTime; } set { _expiryDateTime = value; } }



    }
}
