# Static Files Host App

This application serves static files, such as an Angular application's build output, through an ASP.NET Core web server. The application is configured to use the Serilog library for logging, allowing logs to be output to both the console and a file.

## Features

- Serves static files from the directory specified in the `appsettings.json`.
- Uses Kestrel as the web server and allows configuring the port through `appsettings.json`.
- Logs important events using Serilog, including application startup, shutdown, and serving static files.
- Configurable through the `appsettings.json` file.

## Configuration

The application can be configured using the `appsettings.json` file:

- **StaticFileSettings:RootPath**: Specifies the path to the static files to be served. By default, it is set to `"wwwroot/dist/"`.
- **Serilog**: Configures the logging settings, including logging level and the location of the log files.

## How to Run

1. Modify the `appsettings.json` file if needed, setting the `RootPath` to point to your Angular build output.
2. Use the command `dotnet run` to start the application. It will serve the static files from the specified `RootPath`.

## Logs

- The application logs information about startup, shutdown, and file serving paths.
- Logs are saved to the `logs` directory in the application's root folder, with daily rolling intervals.

## Dependencies

- ASP.NET Core
- Serilog for logging