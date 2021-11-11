namespace JobAgentClassLibrary.Common.Users.Entities
{
    public interface IHashedUser
    {
        public string Password { get; }
        public string Salt { get; }
    }
}
