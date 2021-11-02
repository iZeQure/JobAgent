namespace JobAgentClassLibrary.Core.Entities
{
    public enum LogSeverity
    {
        Trace, Debug, Information, Warning, Error, Critical
    }

    public enum DbConnectionType
    {
        Basic,
        Complex,
        Create,
        Update,
        Delete
    }
}
