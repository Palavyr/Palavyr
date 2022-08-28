param([bool]$showEnv = $false)

Clear-Host

Write-Host ""
Write-Host "PALAVYR DEVELOPMENT ENVIRONMENT SETUP SCRIPT" -ForegroundColor DarkYellow
Write-Host ""

try {
    $processes = Get-Process "*docker desktop*"
    if ($processes.Count -eq 0) {
        #     $processes[0].CloseMainWindow();
        #     $processes[0].Kill();
        #     $processes[0].WaitForExit();
        Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe" -NoNewWindow -WindowStyle Hidden
    }
}
catch {
    Write-Error "Couldn't auto-start docker - make sure thats running or this script will fail."
    # exit 1
}

try {
    Write-Host "Checking if AWS CLI is installed" -ForegroundColor DarkYellow
    Write-Host ""
    Write-Host "Found $(aws --version)"
}
catch {
    # https://docs.aws.amazon.com/cli/latest/userguide/awscli-install-windows.html
    $dlurl = "https://s3.amazonaws.com/aws-cli/AWSCLI64PY3.msi"
    $installerPath = Join-Path $env:TEMP (Split-Path $dlurl -Leaf)
    $ProgressPreference = 'SilentlyContinue'
    Invoke-WebRequest $dlurl -OutFile $installerPath
    Start-Process -FilePath msiexec -Args "/i $installerPath /passive" -Verb RunAs -Wait
    Remove-Item $installerPath
    $env:Path += ";C:\Program Files\Amazon\AWSCLI\bin"
}

# Log in to the ECR registry
# Please ensure you've populated your local .env file
# If you haven't, make a copy of the .env.Template file and populate it


if ($showEnv -eq $true) {

    Write-Host "Using the following env variables" -ForegroundColor DarkYellow
    Write-Host ""

    Get-Content local.env | ForEach-Object {
        Write-Host $_
        $name, $value = $_.split('=')
        Set-Content env:\$name $value
    }
}

Write-Host ""
Write-Host "Grabbing your AWS Elastic Container Registry credentials... if this fails, you'll need to request aws credentials to access ECR" -ForegroundColor DarkYellow
Write-Host ""
aws ecr get-login-password --region "us-east-1" | docker login --username AWS --password-stdin $env:ECR_REGISTRY
Write-Host ""


Write-Host "Composing your docker environment..." -ForegroundColor DarkYellow
docker compose down
docker volume prune -f

docker compose pull
docker compose -f ./docker-compose.yml up -d --remove-orphans --force-recreate
Write-Host ""


Write-Host "Moving to the utilities directory" -ForegroundColor DarkYellow
Set-Location ./server
Write-Host ""
try {
    Write-Host "Applying migrations..." -ForegroundColor DarkYellow
    Write-Host ""
    ./utilities/StartMigrator.ps1
    Write-Host ""
}
catch {
    exit 1
}
finally {
    Set-Location ..
}

Write-Host "Tearing down docker environment temporarily so we can set up dev account..." -ForegroundColor DarkYellow

docker compose down
docker compose -f ./docker-compose.setup.yml up -d

Write-Host "Attempting to create your admin dev account" -ForegroundColor DarkYellow

try {
    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $headers.Add("action", "login")
    $headers.Add("Content-Type", "application/json")

    $body = "{
    `n    `"EmailAddress`": `"admin@palavyr.com`",
    `n    `"Password`": `"123`"
    `n}"

    try {
        $response = Invoke-RestMethod 'http://localhost:5000/api/account/create/default' -Method 'POST' -Headers $headers -Body $body
        $response | ConvertTo-Json | Write-Host -ForegroundColor Gray
    }
    catch {
        $_.Exception.Message
    }
}
catch {
    $r = $_.Exception
    Write-Error $r;
    exit 1
}


try {
    # If this does not work, then
    $sesResultsUri = "http://localhost:4566/_localstack/ses/"
    $response = Invoke-WebRequest -Method "GET" -Uri $sesResultsUri
    Write-Host "Retrieving the registration email from localstack..." -ForegroundColor DarkYellow
    Write-Host $response -ForegroundColor DarkCyan
}
catch {
    Write-Error "The account was requested successfully, but we weren't able to retrieve the ses email from storage. Retrieve this manually to determine your auth token when loggingin with admin@palavyr.com, password: 123"
}

docker compose down
docker compose -f ./docker-compose.yml up -d --remove-orphans --force-recreate

Write-Host "You're good to go start the app now!" -ForegroundColor DarkYellow
Write-Host ""
Write-Host "If the dev account request succeeded, use the auth token to activate your dev account. TODO: Make the account full access" -ForegroundColor DarkYellow
Write-Host "If the dev account request failed, you'll to manually retrieve the ses email record from local stack. Check this script for details." -ForegroundColor DarkYellow
