param([string]$pass = "0987654321", [string]$user = "postgres")

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
    Write-Host "Setting Connection Strings for $projectPath..."
    dotnet user-secrets set ConnectionStrings:DevAccountsContextPostgres "Server=$server;Port=$port;Database=Accounts;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:DevConvoContextPostgres "Server=$server;Port=$port;Database=Conversations;User Id=$user;Password=$pass" --project $projectPath
    dotnet user-secrets set ConnectionStrings:DevDashContextPostgres "Server=$server;Port=$port;Database=Configuration;User Id=$user;Password=$pass" --project $projectPath
    
}

Write-Host "Clearing previous Secrets"
dotnet user-secrets clear --project $api;
dotnet user-secrets clear --project $migrator;

WriteSecrets($api)
WriteSecrets($migrator)