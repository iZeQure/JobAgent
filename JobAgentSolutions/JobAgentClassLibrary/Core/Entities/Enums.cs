namespace JobAgentClassLibrary.Core.Entities
{
    public enum LogSeverity
    {
        Trace, Debug, Information, Warning, Error, Critical
    }

    public enum DbCredentialType
    {
        BasicUser,
        ComplexUser,
        CreateUser,
        UpdateUser,
        DeleteUser
    }
}
