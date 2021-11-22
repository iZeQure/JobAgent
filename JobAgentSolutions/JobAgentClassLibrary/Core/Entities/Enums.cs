﻿namespace JobAgentClassLibrary.Core.Entities
{
    public enum LogSeverity
    {
        EMERGENCY, ALERT, CRITICAL, ERROR, WARNING, NOTIFICATION, INFO, DEBUG
    }

    public enum LogType
    {
        DATABASE, CRAWLER
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
