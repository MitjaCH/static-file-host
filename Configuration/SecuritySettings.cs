namespace StaticFileHost.Configuration
{
    public class SecuritySettings
    {
        public RateLimitingSettings RateLimiting { get; set; } = new RateLimitingSettings();
    }

    public class RateLimitingSettings
    {
        public int MaxRequestsPerMinute { get; set; }
    }
}
