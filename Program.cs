using System.Diagnostics;
using Serilog;

public class Program
{
    public static void Main(string[] args)
    {
        var isService = !(Debugger.IsAttached || args.Contains("--console"));

        if (isService)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule?.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            if (!string.IsNullOrEmpty(pathToContentRoot))
            {
                Directory.SetCurrentDirectory(pathToContentRoot);
            }
        }

        var hostBuilder = CreateHostBuilder(args.Where(arg => arg != "--console").ToArray());

        if (isService)
        {
            hostBuilder.UseWindowsService().Build().Run();
        }
        else
        {
            var webHost = new WebHost(args);
            webHost.Run();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseSerilog();
}
