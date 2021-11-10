using JobAgentClassLibrary.Common.Users.Entities;

namespace JobAgentClassLibrary.Security.Cryptography
{
    public interface ICryptographyService
    {
        IHashedUser CreateHashedUser(string password);

        string HashUserPasswordWithSalt(string password, string salt);
    }
}
