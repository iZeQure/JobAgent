namespace JobAgentClassLibrary.Core.Settings
{
    public class DbConnectionSettings : IConnectionSettings
    {
        public string ConnectionString { get; set; }

        public string ServerHost { get; set; }

        public string Database { get; set; }
    }
}
