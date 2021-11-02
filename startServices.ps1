## SETUP SCRIPT ##
Write-Host "This script will start the frontend and pdf server for development".

$scriptLocation = Split-Path $MyInvocation.MyCommand.Path -Parent
Write-Host "Executing from $scriptLocation"

try {
    git clone https://github.com/Palavyr/PalavyrPdfServer
}
catch {
    # do nothing
}
$pdfServer = "$scriptLocation/PalavyrPdfServer"

try {

    npm i -G serverless
}
catch {
    # do nothing
}



# Build and Start node express server
Set-Location $pdfServer

npm i
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-Nam", "-noexit", "-command Set-Location $pdfServer; npm start"

Set-Location $scriptLocation

# Start the frontend
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-noexit", "-command Set-Location $scriptLocation/ui; npm start"

# Start the widget
Start-Process PowerShell.exe -ArgumentList "-WindowStyle Minimized", "-noexit", "-command Set-Location $scriptLocation/ui; npm run start-widget"

# return to base dir
Set-Location $scriptLocation

