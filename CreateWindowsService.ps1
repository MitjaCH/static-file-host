# Variables
$serviceName = "StaticFileHostService"
$appPath = "C:\Path\To\YourApp\YourApp.exe"

# Check if the service already exists
if (Get-Service -Name $serviceName -ErrorAction SilentlyContinue) {
    Write-Host "Service $serviceName already exists. Stopping and removing it..."
    Stop-Service -Name $serviceName -Force
    sc.exe delete $serviceName
    Start-Sleep -Seconds 2
}

# Create the new service
sc.exe create $serviceName binPath= "\"$appPath\"" start= auto
Write-Host "Service $serviceName created successfully."

# Start the service
Start-Service -Name $serviceName
Write-Host "Service $serviceName started successfully."