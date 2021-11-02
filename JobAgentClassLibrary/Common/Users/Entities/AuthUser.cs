namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class AuthUser : User, IAuthUser
    {
        public string Password { get; set; }

        public string Salt { get; set; }

        public string AccessToken { get; set; }
    }
}
