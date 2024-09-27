namespace StaticFileHost.Configuration
{
    public class StaticFileSettings
    {
        public string RootPath { get; set; } = string.Empty;
        public bool EnableDirectoryBrowsing { get; set; }
        public List<string> DefaultFiles { get; set; } = new List<string>();
        public CacheSettings CacheSettings { get; set; } = new CacheSettings();
        public List<string> AllowedIPs { get; set; } = new List<string>();
    }

    public class CacheSettings
    {
        public bool EnableCaching { get; set; }
        public int CacheDurationInSeconds { get; set; }
    }
}
