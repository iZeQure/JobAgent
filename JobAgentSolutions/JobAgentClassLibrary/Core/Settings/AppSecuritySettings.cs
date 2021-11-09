namespace JobAgentClassLibrary.Core.Settings
{
    public class AppSecuritySettings : ISecuritySettings
    {
        public string JwtSecurityKey { get; set; }
    }
}
