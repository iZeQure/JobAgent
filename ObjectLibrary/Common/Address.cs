using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Handles the information of an address.
    /// </summary>
    public class Address : BaseEntity
    {
        private string _streetAddress;
        private string _city;
        private string _country;
        private string _postalCode;

        public Address(int jobAdvertVacantJobId, string streetAddress, string city, string country, string postalCode) : base (jobAdvertVacantJobId)
        {
            _streetAddress = streetAddress;
            _city = city;
            _country = country;
            _postalCode = postalCode;
        }

        /// <summary>
        /// Contains the specific Street Address.
        /// </summary>
        public string StreetAddress { get { return _streetAddress; } set { _streetAddress = value; } }

        /// <summary>
        /// The city of which the address is located.
        /// </summary>
        public string City { get { return _city; } set { _city = value; } }

        /// <summary>
        /// The country of which the city is in.
        /// </summary>
        public string Country { get { return _country; } set { _country = value; } }

        /// <summary>
        /// The postal code of which the city is associated to.
        /// </summary>
        public string PostalCode { get { return _postalCode; } set { _postalCode = value; } }
    }
}
