using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public class User : BaseEntity, IUser
    {
        private readonly Role _userRole;
        private readonly Location _userLocation;
        private readonly List<Area> _consultantAreas;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private readonly string _password;
        private readonly string _salt;
        private readonly string _accessToken;

        public User(int id, Role userRole, Location userLocation, List<Area> consultantAreas, string firstName, string lastName, string email, string password = null, string salt = null, string accessToken = null) : base(id)
        {
            _userRole = userRole;
            _userLocation = userLocation;
            _consultantAreas = consultantAreas;
            _firstName = firstName;
            _lastName = lastName;
            _email = email;
            _password = password;
            _salt = salt;
            _accessToken = accessToken;
        }

        /// <summary>
        /// Get the user secret.
        /// </summary>
        public string Password { get { return _password; } }

        /// <summary>
        /// Get the auto generated salt for the user.
        /// </summary>
        public string Salt { get { return _salt; } }

        /// <summary>
        /// Get the auto generated access token for the user.
        /// </summary>
        public string AccessToken { get { return _accessToken; } }

        /// <summary>
        /// Get the role of the user.
        /// </summary>
        public Role GetRole { get { return _userRole; } }

        /// <summary>
        /// Get the location of where the user is located at.
        /// </summary>
        public Location GetLocation { get { return _userLocation; } }

        /// <summary>
        /// Get the areas of which the user is consulting at.
        /// </summary>
        public IEnumerable<Area> GetConsultantAreas { get { return _consultantAreas; } }

        /// <summary>
        /// Get the full name of the user.
        /// </summary>
        public string GetFullName { get { return $"{_firstName} {_lastName}"; } }

        /// <summary>
        /// Get the email for the user.
        /// </summary>
        public string GetEmail { get { return _email; } }
    }
}
