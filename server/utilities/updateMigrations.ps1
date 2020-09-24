param([string]$name, [string]$version, [switch]$migrate)

$env:ASPNETCORE_ENVIRONMENT = "Development"


Write-Host "Run this script from the server directory (next to the project)"
Write-Host "ONLY RUN THIS IN DEV TO PRODUCE MIGRATION SCRIPTS!!"
## This script is used to produce a unified migration script for each of contexts used with Palavyr ##
## The general idea is to produce an idempotent script at each migration so that we can avoid worrying about the migration
## state of a given stage
##
## e.g.             migration
## Dev:       ||||||||||||||||||||||||
## Staging:   |||||||||||||||
## Prod:      ||||||||||
## 
## Using this approach, we will always produce the database at the furthest migration (in this case, Dev). And since we hold our
## migration script with our code, DBUP will automatically handle applying these scripts for us.
# https://www.red-gate.com/simple-talk/sysadmin/powershell/how-to-use-parameters-in-powershell-part-ii/

## Steps:
## 1. Run the dotnet ef core migration command on each DBContext
## 2. Run the Update-Database command to bring the local db up to date
## 3. Scan the scripts diretory to determine the latest number
## 4. Run the script generation with the idempotent flag to 

############################################################
##Fucnctions
function CheckDirForExistingVersions($path) {
    if (Test-Path $path) {
        $files = Get-ChildItem $path
        foreach ($fi in $files) {
            if ($fi.Name.Split("-")[0] -eq $version) {
                Write-Host "Version $version already used in $path"
                exit
            }
        }
    }
    else {
        Write-Host "Perhaps $path does not exist..."
    }
}

##
$Dir = ".\\Palavyr.Data.Migrator\\Scripts"
$accountsOutput = "$Dir\\Account" 
$configOutput = "$Dir\\Config"
$convoOutput = "$Dir\\Convo"

CheckDirForExistingVersions($accountsOutput)
CheckDirForExistingVersions($configOutput)
CheckDirForExistingVersions($convoOutput)

if ($name -eq "") {
    Write-Host "Name arg is required. Choose a sensible descriptive name. Long names are okay."
    exit
}
if ($version -eq "") {
    Write-Host "Version arg is required. should be like '0001'. Check scripts directory for current version and increment by 1."
    exit
}

## make sure the tool is installed globally
try {
    dotnet tool install --global dotnet-ef --version=3.1.0
}
catch {
    Write-Host "dotnet ef tool already installed" -ForegroundColor RED
}

$Migrations = "Migrations"

# Execute the migrations on the local space
try {
    Write-Host "`r`nAdding migration for Accounts"
    dotnet-ef migrations add $name -p .\Palavyr.Data\ -s .\Palavyr.API\ -o "$Migrations\\AccountsMigrations" -c AccountsContext
    $AccountsResult = $?;
}
catch {
    write-host "Accounts Migration for $name not applied"
    $AccountsResult = $?;
}

try {
    Write-Host "`r`nAdding migration for Configuration"
    dotnet-ef migrations add $name -p .\Palavyr.Data\ -s .\Palavyr.API\ -o "$Migrations\\ConfigurationMigrations" -c DashContext
    $ConfigResult = $?;
}
catch {
    write-host "Configuraton migration for $name not applied"
    $ConfigResult = $?;
}

try {
    Write-Host "`r`nAdding migration for Convos"
    dotnet-ef migrations add $name -p .\Palavyr.Data\ -s .\Palavyr.API\ -o "$Migrations\\ConvoMigrations" -c ConvoContext
    $ConvoResult = $?;
}
catch {
    write-host "Configuration migration for $name not applied"
    $ConvoResult = $?;
}



Write-Host "`r`nExporting migrations as SQL Scripts..."


if ($AccountsResult) {
    Write-Host "`r`nExporting Accounts Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $accountsOutput\\$version-accounts_migration-$name.sql"
    dotnet ef migrations script --project ./Palavyr.API --context AccountsContext --output "$accountsOutput\\$version-accounts_migration-$name.sql" --idempotent    
}
else {
    Write-Host "`r`nNot creating sql script for Accounts DB - no new migrations.";
}

if ($ConfigResult) {
    Write-Host "`r`nExporting Configuration Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $configOutput\\$version-configuration_migration-$name.sql"
    dotnet ef migrations script --project ./Palavyr.API --context DashContext --output "$configOutput\\$version-configuration_migration-$name.sql" --idempotent
}
else {
    Write-Host "`r`nNotCreating sql script for Config DB - no new migrations."
}

if ($ConvoResult) {
    Write-Host "`r`nExporting Conversation Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $convoOutput\\$version-convo_migration-$name.sql"
    dotnet ef migrations script --project ./Palavyr.API --context ConvoContext --output "$convoOutput\\$version-convo_migration-$name.sql" --idempotent
}
else {
    Write-Host "`r`nNotCreating sql script for Config DB - no new migrations."

}

if ($migrate) {
    Write-Host "`r`nMigrating the local database using DBUP!`r`n"
    & "$PSScriptRoot\\StartMigrator.ps1"
}

Write-Host "`r`n`r`nAll set! You can check these new scripts into Git and deploy them with the server!`r`n"
