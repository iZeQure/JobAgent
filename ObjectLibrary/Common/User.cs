using ObjectLibrary.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Common
{
    public class User : IUser
    {
        private readonly Role _userRole;
        private readonly Location _userLocation;
        private readonly List<Area> _consultantAreas;
        private readonly string _firstName;
        private readonly string _lastName;
        private readonly string _email;
        private string _password;
        private string _salt;
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

        public int GetUserId { get { return Id; } }

        /// <summary>
        /// Get the user secret.
        /// </summary>
        public string GetPassword { get { return _password; } }

        /// <summary>
        /// Get the auto generated salt for the user.
        /// </summary>
        public string GetSalt { get { return _salt; } }

        /// <summary>
        /// Get the auto generated access token for the user.
        /// </summary>
        public string GetAccessToken { get { return _accessToken; } }

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
        /// Get the firstname of the user.
        /// </summary>
        public string GetFirstName { get { return $"{_firstName}"; } }


        /// <summary>
        /// Get the lastname of the user.
        /// </summary>
        public string GetLastName { get { return $"{_lastName}"; } }

        /// <summary>
        /// Get the full name of the user.
        /// </summary>
        public string GetFullName { get { return $"{_firstName} {_lastName}"; } }

        /// <summary>
        /// Get the email for the user.
        /// </summary>
        public string GetEmail { get { return _email; } }

        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void SetPassword(string password)
        {
            _password = password;
        }

        public void SetSalt(string salt)
        {
            _salt = salt;
        }
    }
}
