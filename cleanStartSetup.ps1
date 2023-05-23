param([switch]$showEnv = $false)
param([switch]$local = $true)

Clear-Host
Write-Host ""
Write-Host "PALAVYR DEVELOPMENT ENVIRONMENT SETUP SCRIPT" -ForegroundColor DarkYellow
Write-Host ""

try {
    Import-Module AWSPowerShell.NetCore
}
catch {
    Write-Error "AWSPowerShell.NetCore is not installed (⊙o⊙)"
    Install-Module -Name AWSPowerShell.NetCore
}
finally {
    try {
        Import-Module AWSPowerShell.NetCore
    }
    catch {
        Write-Error "Failed to import AwsPowershell.NetCore, even after installing it"
        exit 1
    }
}

try {
    pip --version
}
catch {
    Write-Error "You need to install python for this script to work"
    exit 1
}

$awslocalFound = 0
try {
    awslocal --version
    $awslocalFound = 1
}
catch {
    pip install awscli-local
}
finally {
    if ($awslocalFound -eq 0) {
        awslocal --version
    }
}

try {
    $processes = Get-Process "*docker desktop*"
    if ($processes.Count -eq 0) {
        Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe" -NoNewWindow

    }
}
catch {
    Write-Error "Couldn't auto-start docker - make sure thats running or this script will fail."
}

try {
    Write-Host "Checking if AWS CLI is installed" -ForegroundColor DarkYellow
    Write-Host ""
    Write-Host "Found $(aws --version)"
}
catch {

    Invoke-WebRequest -Uri https://awscli.amazonaws.com/AWSCLIV2.msi -OutFile awscliv2.msi
    Start-Process msiexec.exe -Wait -ArgumentList '/I awscliv2.msi'
    Remove-Item -Force awscliv2.msi

    $env:Path += ";C:\Program Files\Amazon\AWSCLI\bin"
}

if ($showEnv -eq $true) {

    Write-Host "Using the following env variables" -ForegroundColor DarkYellow
    Write-Host ""

    Get-Content local.env | ForEach-Object {
        Write-Host $_
        $name, $value = $_.split('=')
        Set-Content env:\$name $value
    }
}

Write-Host "Composing your docker environment..." -ForegroundColor DarkYellow
docker compose down
docker volume prune -f

docker compose pull
docker compose -f ./docker-compose.yml up -d --remove-orphans --force-recreate
Write-Host ""

Write-Host $local
Read-Host
if ($local -eq $true)
{
    Read-Host "Please stop your local running server (if its on)..."
}


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


Write-Host "Creating local stack configuration identity and secret" -ForegroundColor DarkYellow
aws configure set aws_access_key_id default_access_key --profile=localstack --endpoint-url=http://localhost:4566
aws configure set aws_secret_access_key default_secret_key --profile=localstack  --endpoint-url=http://localhost:4566
aws configure set region us-east-1 --profile=localstack  --endpoint-url=http://localhost:4566

Write-Host "########### Listing profile ###########" -ForegroundColor DarkYellow
aws configure list --profile=localstack  --endpoint-url=http://localhost:4566

Write-Host "Creating local stack resources and email identities" -ForegroundColor DarkYellow
awslocal --endpoint-url=http://localhost:4566 s3 mb s3://palavyr-user-data-development
awslocal ses verify-email-identity --endpoint-url=http://localhost:4566 --email-address palavyr@gmail.com
awslocal ses verify-email-identity --endpoint-url=http://localhost:4566 --email-address admin@palavyr.com

Write-Host "Listing Current identities for debug"
awslocal ses list-identities --endpoint-url=http://localhost:4566
Start-Sleep -Second 5

if ($local -eq $true)
{
    Read-Host "Please start your local running server (if its off)..."
}


Write-Host "Waiting for the Server to start responding" -ForegroundColor DarkYellow

$ready = $false;
do {

    try {

        $response = Invoke-WebRequest 'http://localhost:5000/healthcheck' -Method GET
    }
    catch {
        # do nothing
    }

    if ($response.Content -eq "Healthy") {
        $ready = $true;
        Write-Host $response
    }

    Start-Sleep -Second 5

} while ($ready -eq $false)

Write-Host "Preparing request for dev account creation..." -ForegroundColor DarkYellow
$headers = @{
    "Content-Type" = "application/json"
    "action"       = "login"
}

$body = @{EmailAddress = 'dev@palavyr.com'; Password = "123" } | ConvertTo-Json
Write-Host "Attempting to create your dev account" -ForegroundColor DarkYellow

$response = Invoke-WebRequest 'http://localhost:5000/api/account/create/default' -Method POST -Headers $headers -Body $body
if ($response.StatusCode -eq 500)
{
    Write-Error "Failed to read the web server!"
    Write-Error $response.Message
    exit 1
}
Write-Host $response -ForegroundColor Gray

$token = $null;
try {
    # If this does not work, then problem
    $sesResultsUri = "http://localhost:4566/_localstack/ses/"
    $response = Invoke-WebRequest -Method GET -Uri $sesResultsUri
    Write-Host "Retrieving the registration email from localstack..." -ForegroundColor DarkYellow
    $token = $response.Content.Split("palavyr.com after you sign in.\n\n")[1].split("\n\nWith that, its smooth sailing!")[0]
    Write-Host "Provide this to your confirmation page when you first log in: $token" -ForegroundColor DarkCyan
}
catch {

    Write-Error "The account was requested successfully, but we weren't able to retrieve the ses email from storage. Retrieve this manually to determine your auth token when loggingin with admin@palavyr.com, password: 123"
    Write-Host $_.Exception.Message
    $r = $_.Exception
    Write-Error $r;
    exit 1
}

docker stop Server
docker stop Database-Migrator

Write-Host "You're good to go start the app now!" -ForegroundColor DarkYellow
Write-Host ""
if ($null -eq $token) {
    Write-Host "If the dev account request succeeded, use the auth token to activate your dev account. TODO: Make the account full access" -ForegroundColor DarkYellow
    Write-Host "If the dev account request failed, you'll to manually retrieve the ses email record from local stack. Check this script for details." -ForegroundColor DarkYellow
}