using System.Collections.Generic;

namespace ObjectLibrary.Common.Abstractions
{
    /// <summary>
    /// Encapsulates the <see cref="User"/> properties for security purposes.
    /// </summary>
    public interface IUser : IBaseEntity<int>
    {
        /// <summary>
        /// Get the user id
        /// </summary>
        int GetUserId { get; }

        /// <summary>
        /// Get the full name of the user entity.
        /// </summary>
        string GetFullName { get; }

        string GetFirstName { get; }

        string GetLastName { get; }

        string GetPassword { get; }

        string GetSalt { get; }

        string GetAccessToken { get; }

        /// <summary>
        /// Get the email of user entity.
        /// </summary>
        string GetEmail { get; }

        /// <summary>
        /// Get the role tied to the user.
        /// </summary>
        Role GetRole { get; }

        /// <summary>
        /// Get the location of which the user is tied to.
        /// </summary>
        Location GetLocation { get; }

        /// <summary>
        /// Get a collection of consultant areas for the user.
        /// </summary>
        IEnumerable<Area> GetConsultantAreas { get; }

        public void SetPassword(string password);

        public void SetSalt(string salt);
    }
}
