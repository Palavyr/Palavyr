Write-Output ""
Write-Output "Welcome to the basic setup script"
Write-Output ""

try {
    Write-Host "Checking if AWS CLI is installed"
    aws --version
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
Write-Output ""
Write-Output "Make sure you've set up your two .env files - on in the root of the project"
Write-Output ""

# Log in to the ECR registry
# Please ensure you've populated your local .env file
# If you haven't, make a copy of the .env.Template file and populate it


Write-Host "Using the following env variables"
Write-Output ""
Get-Content local.env | ForEach-Object {
    Write-Host $_
    $name, $value = $_.split('=')
    Set-Content env:\$name $value
}

Write-Output ""
Write-Output ""
Write-Output "Grabbing your AWS Elastic Container Registry credentials... if this fails, you'll need to request aws credentials to access ECR"
aws ecr get-login-password --region "us-east-1" | docker login --username AWS --password-stdin $env:ECR_REGISTRY
Write-Output ""
Write-Output ""


Write-Output "Composing your docker environment..."
docker compose down
docker volume prune -f

docker compose pull
docker compose -f ./docker-compose.yml up -d --remove-orphans --force-recreate
Write-Output ""


Write-Output "Moving to the utilities directory"
Set-Location ./server
Write-Output ""
try {
    Write-Output "Applying migrations..."
    dotnet build
    ./utilities/StartMigrator.ps1
    Write-Output ""
}
catch {
    exit 1
}
finally {
    Set-Location ..
}

Write-Host "Tearing down docker environment temporarily so we can set up dev account..."

docker compose down
docker compose -f ./docker-compose.setup.yml up -d

try {
    $emailAddress = "admin@palavyr.com";
    $headers = @{'Accept' = 'application/json'; 'action' = "login" };
    $url = "http://localhost:5000/api/account/create/default";
    $body = @{ EmailAddress = $emailAddress; Password = "123"; };

    Write-Host "Setting up the admin dev account...."
    Invoke-WebRequest `
        -Method 'Post' `
        -Uri $url `
        -Headers $headers `
        -Body $body
}
catch {
    $r = $_.Exception
    Write-Error $r;
    exit 1
}

try {
    $sesResultsUri = "http://localhost:4566/_localstack/ses/"
    $response = Invoke-WebRequest -Method "GET" -Uri $sesResultsUri
    Write-Output "Retrieving the registration email from localstack..."
    Write-Host $response
}
catch {
    Write-Error "The account was requested successfully, but we weren't able to retrieve the ses email from storage. Retrieve this manually to determine your auth token when loggingin with admin@palavyr.com, password: 123"
}

docker compose down
docker compose -f ./docker-compose.yml up -d --remove-orphans --force-recreate

Write-Output "You're good to go start the app now!"
Write-Output ""
Write-Output "If you'd like to auto set up a dev account"

