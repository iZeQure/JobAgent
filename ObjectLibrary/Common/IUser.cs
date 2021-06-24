using System.Collections.Generic;

namespace ObjectLibrary.Common
{
    /// <summary>
    /// Encapsulates the <see cref="User"/> properties for security purposes.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Get the user id
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Get the full name of the user entity.
        /// </summary>
        string GetFullName { get; }

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
    }
}
