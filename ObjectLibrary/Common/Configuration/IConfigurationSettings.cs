namespace ObjectLibrary.Common.Configuration
{
    /// <summary>
    /// A wrapper for configurations settings injected by dependency injection.
    /// </summary>
    public interface IConfigurationSettings
    {
        /// <summary>
        /// Gets the connection string for the data service.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Gets the Security key used to create tokens.
        /// </summary>
        string JwtSecurityKey { get; }
    }
}