### Updated `README.md`

# Static Files Host App

This application serves static files, such as an Angular application's build output, through an ASP.NET Core web server. It uses Serilog for logging and can run as a Windows Service.

## Features
- Serves static files from a configurable path.
- Can run as a Windows Service or a standalone application.
- Default HTML templates provided in `wwwroot`.
- Configurable logging using Serilog.

## Configuration

The application can be configured using the `appsettings.json` file:

- **RunAsService**: Set to `true` to run as a Windows Service, or `false` to run normally.
- **StaticFileSettings:RootPath**: Path to the static files (default: `wwwroot`).
- **Serilog**: Configure logging settings (level, file path).
- **ServerSettings**: Configure settings like `Port`, `IPAddress`, `EnableHTTPS`, and SSL certificate paths.

## How to Run

1. Modify the `appsettings.json` file as needed:
   - Set `StaticFileSettings:RootPath` to point to your static files.
   - Set `RunAsService` to `true` if you want to run it as a service.
2. Place your static files in the `wwwroot` folder or replace existing templates (`index.html` and `default.html`).
3. Start the application:
   - As a regular application: Run `dotnet run`.
   - As a Windows Service: Follow the next steps.

## Running as a Windows Service

### Using the Script
1. Update the `CreateWindowsService.ps1` script:
   - Change `$appPath` to your published executable path.
2. Open PowerShell as Administrator and run:
   ```powershell
   .\CreateWindowsService.ps1
   ```
   This will create and start the service automatically.

### Manually
1. Publish the application:
   ```bash
   dotnet publish -c Release -o C:\Path\To\YourApp
   ```
2. Create the service using Command Prompt (Admin):
   ```bash
   sc create MyStaticFileHostService binPath= "C:\Path\To\YourApp\YourApp.exe"
   ```
3. Start the service:
   ```bash
   net start MyStaticFileHostService
   ```

## Logs

- Logs are stored in the `logs` directory with daily rolling intervals.
- Contains startup, shutdown, and file access logs.

## Dependencies
- **ASP.NET Core**: Core framework for hosting.
- **Serilog**: For logging events.
- **Microsoft.Extensions.Hosting.WindowsServices**: For Windows Service support (optional).