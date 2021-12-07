namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class HashedUser : IHashedUser
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
