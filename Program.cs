using Microsoft.Extensions.FileProviders;
using StaticFileHost.Configuration;
using System.Net;
using System.IO;
using Microsoft.Extensions.Hosting.WindowsServices;

var builder = WebApplication.CreateBuilder(args);

// Load configuration with default values
var configuration = builder.Configuration;
var runAsService = configuration.GetValue<bool>("RunAsService");
var serverSettings = configuration.GetSection("ServerSettings").Get<ServerSettings>() ?? new ServerSettings();
var staticFileSettings = configuration.GetSection("StaticFileSettings").Get<StaticFileSettings>() ?? new StaticFileSettings();
var securitySettings = configuration.GetSection("SecuritySettings").Get<SecuritySettings>() ?? new SecuritySettings();

// Check if running as a Windows Service based on configuration
if (runAsService && WindowsServiceHelpers.IsWindowsService())
{
    builder.Host.UseWindowsService();
}

// Ensure wwwroot directory exist
var baseDirectory = AppContext.BaseDirectory;
var wwwrootPath = Path.Combine(baseDirectory, "wwwroot");


builder.WebHost.ConfigureKestrel(options =>
{
    if (serverSettings.EnableHTTPS)
    {
        options.Listen(IPAddress.Parse(serverSettings.IPAddress), serverSettings.Port, listenOptions =>
        {
            listenOptions.UseHttps(serverSettings.SSLCertificatePath, serverSettings.SSLCertificatePassword);
        });
    }
    else
    {
        options.Listen(IPAddress.Parse(serverSettings.IPAddress), serverSettings.Port);
    }
});

var app = builder.Build();

// IP Whitelisting Middleware
app.Use(async (context, next) =>
{
    var remoteIp = context.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
    if (!staticFileSettings.AllowedIPs.Contains(remoteIp))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Access Denied");
        return;
    }
    await next.Invoke();
});

// Static File Middleware with caching and directory browsing
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(Path.GetFullPath(staticFileSettings.RootPath)),
    EnableDirectoryBrowsing = staticFileSettings.EnableDirectoryBrowsing,
    DefaultFilesOptions = { DefaultFileNames = staticFileSettings.DefaultFiles },
    StaticFileOptions = {
        OnPrepareResponse = ctx =>
        {
            if (staticFileSettings.CacheSettings.EnableCaching)
            {
                ctx.Context.Response.Headers["Cache-Control"] = $"public, max-age={staticFileSettings.CacheSettings.CacheDurationInSeconds}";
            }
        }
    }
});

app.Run();
