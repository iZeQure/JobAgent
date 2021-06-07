using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pocos
{
    public class User : BaseEntity
    {
        #region Fields
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string salt;
        private ConsultantArea consultantArea;
        private Location location;

        private bool isAuthenticatedByServer;
        private string accessToken;
        private string refreshToken;
        #endregion

        #region Properties
        /// <summary>
        /// A user's name.
        /// </summary>
        public string FirstName { get { return firstName; } set { firstName = value; } }

        /// <summary>
        /// Defines the user's family name.
        /// </summary>
        public string LastName { get { return lastName; } set { lastName = value; } }

        /// <summary>
        /// Gets the values from a user's name and family name, then combines them.
        /// </summary>
        public string FullName { get { return $"{FirstName} {LastName}"; } }

        /// <summary>
        /// A user's unique email address.
        /// </summary>
        public string Email { get { return email; } set { email = value; } }

        public string Password { get { return password; } set { password = value;} }
        public string Salt { get { return salt; } set { salt = value; } }
        public ConsultantArea ConsultantArea { get { return consultantArea; } set { consultantArea = value; } }
        public Location Location { get { return location; } set { location = value; } }

        public bool IsAuthenticatedByServer { get { return isAuthenticatedByServer; } set { isAuthenticatedByServer = value; } }
        public string AccessToken { get { return accessToken; } set { accessToken = value; } }
        public string RefreshToken { get { return refreshToken; } set { refreshToken = value; } }
        #endregion
    }
}
