## SETUP SCRIPT ##
Write-Host "This script will start the frontend and pdf server for development".

$scriptLocation = Split-Path $MyInvocation.MyCommand.Path -Parent

# Build and Start node express server
Set-Location $scriptLocation/pdf-server/
npm run build
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-Nam", "-noexit", "-command Set-Location $scriptLocation/pdf-server; node ./dist/app.js"

# Start the frontend
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-noexit", "-command Set-Location $scriptLocation/frontend; npm start"

# Start the widget
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-noexit", "-command Set-Location $scriptLocation/widget; npm start"

# return to base dir
Set-Location $scriptLocation

