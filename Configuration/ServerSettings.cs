namespace StaticFileHost.Configuration
{
    public class ServerSettings
    {
        public int Port { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public bool EnableHTTPS { get; set; }
        public string SSLCertificatePath { get; set; } = string.Empty;
        public string SSLCertificatePassword { get; set; } = string.Empty;
    }
}
