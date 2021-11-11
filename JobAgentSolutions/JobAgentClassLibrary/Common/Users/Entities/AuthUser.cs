namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class AuthUser : User, IAuthUser, IHashedUser
    {
        public string AccessToken { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
