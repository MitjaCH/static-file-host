using Serilog;

public class WebHost
{
    private readonly WebApplication _app;
    private readonly IConfiguration _configuration;
    private readonly string _rootPath;

    public WebHost(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureLogging(builder);

        _configuration = builder.Configuration;

        _rootPath = _configuration["StaticFileSettings:RootPath"] ?? "wwwroot/dist/default-path";

        ConfigureServices(builder);

        _app = builder.Build();
    }

    private void ConfigureLogging(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    private void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddSpaStaticFiles(config => config.RootPath = _rootPath);
    }

    private void Configure()
    {
        Log.Information("Application starting up...");

        if (Directory.Exists(_rootPath))
        {
            _app.UseStaticFiles();
            _app.UseSpaStaticFiles();
            _app.UseSpa(spa => spa.Options.SourcePath = _rootPath);
            Log.Information("Serving static files from: {RootPath}", _rootPath);
        }
        else
        {
            Log.Warning("The specified root path '{RootPath}' does not exist. Static files may be unavailable.", _rootPath);
        }

        _app.Lifetime.ApplicationStopped.Register(() => Log.Information("Application is shutting down."));
    }

    public void Run()
    {
        try
        {
            Configure();
            _app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application failed to start.");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
