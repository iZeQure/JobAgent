namespace JobAgentClassLibrary.Common.Users.Entities
{
    public interface IAuthUser
    {
        public string Password { get; }
        public string Salt { get; }
        public string AccessToken { get; }
    }
}
