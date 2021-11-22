namespace JobAgentClassLibrary.Core.Entities
{
    public enum LogSeverity
    {
        EMERGENCY, ALERT, CRITICAL, ERROR, WARNING, NOTIFICATION, INFO, DEBUG
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
