﻿namespace JobAgentClassLibrary.Core.Entities
{
    public enum LogSeverity
    {
        EMERGENCY = 1, ALERT, CRITICAL, ERROR, WARNING, NOTIFICATION, INFO, DEBUG
    }

    public enum LogType
    {
        SYSTEM, CRAWLER
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
