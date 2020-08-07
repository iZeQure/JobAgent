using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data
{
    public class User : BaseEntity
    {
        #region Attributes
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string confirmPassword;
        private string salt;
        private ConsultantArea consultantArea;
        private Location location;

        private bool isAuthenticatedByServer;
        private string accessToken;
        private string refreshToken;
        #endregion

        #region Properties
        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public string Email { get { return email; } set { email = value; } }
        public string Password { get { return password; } set { password = value;} }
        public string ConfirmPassword { get { return confirmPassword; } set { confirmPassword = value; } }
        public string Salt { get { return salt; } set { salt = value; } }
        public ConsultantArea ConsultantArea { get { return consultantArea; } set { consultantArea = value; } }
        public Location Location { get { return location; } set { location = value; } }

        public bool IsAuthenticatedByServer { get { return isAuthenticatedByServer; } set { isAuthenticatedByServer = value; } }
        public string AccessToken { get { return accessToken; } set { accessToken = value; } }
        public string RefreshToken { get { return refreshToken; } set { refreshToken = value; } }
        #endregion
    }
}
