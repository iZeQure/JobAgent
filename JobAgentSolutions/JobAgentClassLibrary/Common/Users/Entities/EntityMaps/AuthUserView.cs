using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Users.Entities.EntityMaps
{
    /// <summary>
    /// Handles the mapping when authenticating a user.
    /// </summary>
    [Table("AuthUserView")]
    public class AuthUserView : AuthUser, IAuthUser
    {
        public int UserId { get; set; }
    }
}
