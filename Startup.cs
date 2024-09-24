using Serilog;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        var rootPath = _configuration["StaticFileSettings:RootPath"] ?? "wwwroot/dist/default-path";
        services.AddSpaStaticFiles(config => config.RootPath = rootPath);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var rootPath = _configuration["StaticFileSettings:RootPath"] ?? "wwwroot/dist/default-path";

        Log.Information("Application starting up...");

        if (Directory.Exists(rootPath))
        {
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa => spa.Options.SourcePath = rootPath);
            Log.Information("Serving static files from: {RootPath}", rootPath);
        }
        else
        {
            Log.Warning("The specified root path '{RootPath}' does not exist. Static files may be unavailable.", rootPath);
        }

        app.ApplicationServices.GetService<IHostApplicationLifetime>()?.ApplicationStopped.Register(() =>
        {
            Log.Information("Application is shutting down.");
        });
    }
}
