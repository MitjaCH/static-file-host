using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var rootPath = builder.Configuration["StaticFileSettings:RootPath"] ?? "wwwroot/dist/default-path";
builder.Services.AddSpaStaticFiles(config => config.RootPath = rootPath);

var app = builder.Build();

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

app.Lifetime.ApplicationStopped.Register(() => Log.Information("Application is shutting down."));

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start.");
}
finally
{
    Log.CloseAndFlush();
}