namespace JobAgentClassLibrary.Core.Settings
{
    public interface IConnectionSettings
    {
        public string ServerHost { get; }
        public string Database { get; }
        public string ConnectionString { get; }
    }
}
