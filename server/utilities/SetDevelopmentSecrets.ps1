param([string]$pass = "0987654321", [string]$user = "postgres", [string]$awsProfile = "default")

### sets the secret password used to connect to the postgres DB in DEV.

## Postgres DB Name: palavyr_dev
## Posgress User Name: dev
## Postgress Password: (pass in as first argument. Something simple, but it must align with our installation of postgres)
## To modify the username/pass, start pgAdmin4 and visit http://127.0.0.1:60026/browser/
## make sure the server is creted and if you need to, create the dev user.

$server = "127.0.0.1";
$port = "5432";
$api = ".\\Palavyr.API";
$migrator = ".\\Palavyr.Data.Migrator";

function WriteSecrets($projectPath) {
    Write-Host "`r`nSetting Connection Strings for $projectPath...`r`n"
    dotnet user-secrets set ConnectionStrings:AccountsContextPostgres "Server=$server;Port=$port;Database=Accounts;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:ConvoContextPostgres "Server=$server;Port=$port;Database=Conversations;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:DashContextPostgres "Server=$server;Port=$port;Database=Configuration;User Id=$user;Password=$pass" --project $projectPath
}

Write-Host "`r`nClearing previous Secrets`r`n"
dotnet user-secrets clear --project $api;
dotnet user-secrets clear --project $migrator;

WriteSecrets($api)
WriteSecrets($migrator)

#################
# WRITE AWS SECRETS
Write-Host "`r`nSetting Secrets for AWS Credentials"
$prof = Get-AWSCredential $awsProfile;
$credentials = $prof.GetCredentials();
$accessKey = $credentials.AccessKey;
$secretKey = $credentials.SecretKey;

dotnet user-secrets set AWS:AccessKey "$accessKey" --project $api
dotnet user-secrets set AWS:SecretKey "$secretKey" --project $api
dotnet user-secrets set AWS:Region "ap-southeast-2" --project $api
