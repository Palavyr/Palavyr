param([string]$pass = "0987654321", [string]$user = "postgres", [string]$awsProfile = "palavyr", [string] $secretKey = "", [string] $accessKey = "", [string] $region = "")

### sets the secret password used to connect to the postgres DB in DEV.

## Postgres DB Name: palavyr_dev
## Posgress User Name: dev
## Postgress Password: (pass in as first argument. Something simple, but it must align with our installation of postgres)
## To modify the username/pass, start pgAdmin4 and visit http://127.0.0.1:60026/browser/
## make sure the server is creted and if you need to, create the dev user.
## locally, these are located at something like: %APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json
## https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows



Write-Host "Powershell version: $PSVersionTable.PSVersion";

$serverPath = ($PSCommandPath).Replace("utilities\SetDevelopmentSecrets.ps1", "")

# $server = "dev-palavyr-database.clznnuwhyqf6.us-east-1.rds.amazonaws.com";
$server = "127.0.0.1";
$port = "5432";
$api = Join-Path -Path $serverPath -ChildPath "Palavyr.API";
$migrator = Join-Path -Path $serverPath -ChildPath "Palavyr.Data.Migrator";
$integrationTests = Join-Path -Path $serverPath -ChildPath "Palavyr.IntegrationTests";

Write-Host "`r`nSetting Secrets for AWS Credentials for $awsProfile"

if ($secretkey -eq "" -and $accessKey -eq "") {
    Get-Module -Name AWSPowerShell.NetCore
    Import-Module AWSPowerShell.NetCore
    Write-Host "Setting AWS secrets Automagically"
    $credentials = (Get-AWSCredential $awsProfile).GetCredentials();
    $accessKey = $credentials.AccessKey;
    $secretKey = $credentials.SecretKey;
}
if ($region -eq "") {
    $region = 'us-east-1'
}

function WriteDatabaseSecrets($projectPath) {
    Write-Host "`r`nSetting Connection Strings`r`n"
    dotnet user-secrets set ConnectionStrings:AccountsContextPostgres "Server=$server;Port=$port;Database=Accounts;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:ConvoContextPostgres "Server=$server;Port=$port;Database=Conversations;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:DashContextPostgres "Server=$server;Port=$port;Database=Configuration;User Id=$user;Password=$pass" --project $projectPath
}

function WriteAWSSecrets($projectPath) {
    Write-Host "`r`nSetting AWS Secrets`r`n"
    dotnet user-secrets set AWS:AccessKey "$accessKey" --project $projectPath
    dotnet user-secrets set AWS:SecretKey "$secretKey" --project $projectPath
    dotnet user-secrets set AWS:Region "$region" --project $projectPath
}

Write-Host "`r`nClearing previous Secrets`r`n"
dotnet user-secrets clear --project $api;
dotnet user-secrets clear --project $migrator;
dotnet user-secrets clear --project $integrationTests

##################
# Write Migrator Environment

Write-Host "`r`nSetting Migrator secrets ($migrator)`r`n"
dotnet user-secrets set Environment "Development" --project $migrator;
WriteDatabaseSecrets($migrator)



Write-Host "`r`nSetting API secrets ($api)`r`n"
WriteDatabaseSecrets($api)
WriteAWSSecrets($api);

Write-Host "`r`nSetting Integration Test secrets ($api)`r`n"
WriteAWSSecrets($integrationTests);
###################
# Write JWT Secrets (Json Web Token...?)
# dotnet user-secrets set JwtToken:Issuer "http://localhost:8080/" --project $api
# dotnet user-secrets set JwtToken:SecretKey "tobySuperSecretKey" --project $api
dotnet user-secrets set JWTSecretKey "SomeSecretKey345345345345ThatIsITagkhjasdhjsf" --project $api

### STRIPE
$stripeKey = (Get-Item -Path Env:PalavyrStipeSecretKey).Value
dotnet user-secrets set Stripe:SecretKey $stripeKey --project $api
# Clear-Host