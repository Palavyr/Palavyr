param([bool]$showEnv = $false)

Clear-Host
Write-Host ""
Write-Host "PALAVYR DEVELOPMENT ENVIRONMENT SETUP SCRIPT" -ForegroundColor DarkYellow
Write-Host ""

try {
    Get-Module -name AWSPowerShell.NetCore -Confirm A -Force -PassThru
}
catch {
    Write-Host "AWSPowerShell.NetCore is already installed (⊙o⊙)"
} finally {
    try {
        Import-Module AWSPowerShell.NetCore
    } catch {
        # do nothing
    }
}

try {
    $awsCreds = Get-AWSCredential -ProfileName "palavyr_ecr";
} catch {
    $r = $_.Exception
    Write-Error $r;
    exit 1
}

if ($env:ECR_REGISTRY -eq "") {
    Write-Error "You need to set '$ env: ECR_REGISTRY' "
    exit 1
}

if ($null -eq $awsCreds) {
    Write-Error "Ensure you'set your .aws/credentials with a [palavyr_ecr] profile and an ./aws/config with region=us-east-1"
    exit 1
}

try {
    $processes = Get-Process "*docker desktop*"
    if ($processes.Count -eq 0) {
        Start-Process "C:\Program Files\Docker\Docker\Docker Desktop.exe" -NoNewWindow -WindowStyle Hidden
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
    # https://docs.aws.amazon.com/cli/latest/userguide/awscli-install-windows.html
    $dlurl = "https://s3.amazonaws.com/aws-cli/AWSCLI64PY3.msi"
    $installerPath = Join-Path $env:TEMP (Split-Path $dlurl -Leaf)
    $ProgressPreference = 'SilentlyContinue'
    Invoke-WebRequest $dlurl -OutFile $installerPath
    Start-Process -FilePath msiexec -Args "/i $installerPath /passive" -Verb RunAs -Wait
    Remove-Item $installerPath
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

Write-Host ""
Write-Host "Grabbing your AWS Elastic Container Registry credentials... if this fails, you'll need to request aws credentials to access ECR" -ForegroundColor DarkYellow
Write-Host ""
aws ecr get-login-password --region "us-east-1" --profile "palavyr_ecr" | docker login --username AWS --password-stdin $env:ECR_REGISTRY
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

Write-Host "Creating local stack configuration identity and secret" -ForegroundColor DarkYellow
aws configure set aws_access_key_id default_access_key --profile=localstack --endpoint-url=http://localhost:4566
aws configure set aws_secret_access_key default_secret_key --profile=localstack  --endpoint-url=http://localhost:4566
aws configure set region us-east-1 --profile=localstack  --endpoint-url=http://localhost:4566

Write-Host "########### Listing profile ###########" -ForegroundColor DarkYellow
aws configure list --profile=localstack  --endpoint-url=http://localhost:4566

Write-Host "Creating local stack resources and email identities" -ForegroundColor DarkYellow
aws --endpoint-url=http://localhost:4566 s3 mb s3://palavyr-user-data-development
aws ses verify-email-identity --endpoint-url=http://localhost:4566 --email-address palavyr@gmail.com
aws ses verify-email-identity --endpoint-url=http://localhost:4566 --email-address admin@palavyr.com

Write-Host "Listing Current identities for debug"
aws ses list-identities --endpoint-url=http://localhost:4566
Start-Sleep -Second 2

Write-Host "Preparing request for dev account creation..." -ForegroundColor DarkYellow
$headers = @{
    "Content-Type" = "application/json"
    "action"       = "login"
}

$body = @{EmailAddress = 'dev@palavyr.com'; Password = "123" } | ConvertTo-Json
Write-Host "Attempting to create your dev account" -ForegroundColor DarkYellow
$response = Invoke-WebRequest 'http://localhost:5000/api/account/create/default' -Method POST -Headers $headers -Body $body
Write-Host $response -ForegroundColor Gray

$token = $null;
try {
    # If this does not work, then
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
if ($null -eq $token)
{
    Write-Host "If the dev account request succeeded, use the auth token to activate your dev account. TODO: Make the account full access" -ForegroundColor DarkYellow
    Write-Host "If the dev account request failed, you'll to manually retrieve the ses email record from local stack. Check this script for details." -ForegroundColor DarkYellow
}

