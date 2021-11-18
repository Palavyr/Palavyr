param([string]$name, [switch]$supressmigration)

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

# The way migrations work

# There are 4 pieces to the migration process:
# 1. The Add-Migration call (dotnet ef cli tool) - this will
#     a) add a migration .cs given the model changes (the class changes you make in the code)
#     b) make a change to the model snapshot .cs file (confirmed this by manually running the migration)
# 2. The Update-Database call which will
#     a) apply the migration to the database (add cols, drop cols, add tables, etc based on the code changes you made)
#     b) write a new record in the database in the _EFMigrationsHistory table
# 3. The `dotnet ef migrations script` call, which will produce an SQL script that can be applied to the database (IN PLACE OF STEP 2)
# 4. The DBUP console app StartMigrator call which runs the DBUP console app and applies the SQL script from step 3 which
#     a) applies the migration to the database
#     b) write a new record to the SchemaVersions table to keep track of which scripts have been run.

# In this script, we use steps 1, 3, and 4 to migrate the database.

# IF YOU NEED TO ROLL BACK
# 1. Undo schema changes in the code
# 2. Delete the migration
# 3. Step back the changes to the snapshot .cs file (DO NOT DELETE EVERYTHING. Otherwise the whole diff gets applied next update and it will fail)
# 4. Delete the last update to the EFMigrationsHistory table
# 5. Delete the last update to the SchemaVersions table.
# 6. Delete any scripts you've created (if you've embedded them, delete the records from the csproj file)

# This will effectively put you back before the changes were made.

# If you've applied changes to the tables, you can manually delete columns.

############################################################
##Functions
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

function GetNextMigrationScriptVersion() {
    $dataMigratorDirectory = "./Palavyr.Data.Migrator/Scripts/Account"; # use account because no particular reason

    [int[]]$allVersions = @();

    $files = get-childitem $dataMigratorDirectory;
    foreach ($file in $files) {
        $numberString = ($file.Name.Split("-")[0].Replace("Script", "")).TrimStart("0");
        $version = [int]$numberString;
        $allVersions += $version;
    }
    $sortedVersions = $allVersions | Sort-Object -descending;
    $latestVersion = $sortedVersions[0];

    $nextVersion = $latestVersion + 1;

    $latestVersionAsString = $([string]$latestVersion).PadLeft(4, "0");
    $nextVersionAsString = $([string]$nextVersion).PadLeft(4, "0");

    Write-Host "The current migration Script version is $latestVersionAsString";
    Write-Host "The next version is: $nextVersionAsString"

    return $nextVersionAsString;
}

$nextMigrationScriptVersion = GetNextMigrationScriptVersion();


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

## CLEAN AND BUILD
dotnet clean
dotnet build

$Migrations = "Data\\Migrations"

# Execute the migrations on the local space
try {
    Write-Host "`r`nAdding migration for Accounts"
    dotnet ef migrations add $name --project .\\Palavyr.Core --startup-project .\\Palavyr.API --output-dir "$Migrations\\AccountsMigrations" --context AccountsContext
    $AccountsResult = $?;
}
catch {
    write-host "Accounts Migration for $name not applied"
    $AccountsResult = $?;
}

try {
    Write-Host "`r`nAdding migration for Configuration at: "
    dotnet ef migrations add $name --project .\\Palavyr.Core --startup-project .\\Palavyr.API --output-dir "$Migrations\\ConfigurationMigrations" --context DashContext
    $ConfigResult = $?;
}
catch {
    write-host "Configuraton migration for $name not applied"
    $ConfigResult = $?;
}

try {
    Write-Host "`r`nAdding migration for Convos"
    dotnet ef migrations add $name --project .\\Palavyr.Core --startup-project .\\Palavyr.API --output-dir "$Migrations\\ConvoMigrations" --context ConvoContext
    $ConvoResult = $?;
}
catch {
    write-host "Configuration migration for $name not applied"
    $ConvoResult = $?;
}


Write-Host "`r`nExporting migrations as SQL Scripts..."


if ($AccountsResult) {
    Write-Host "`r`nExporting Accounts Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $accountsOutput\\$nextMigrationScriptVersion-accounts_migration-$name.sql"
    dotnet ef migrations script --project .\\Palavyr.Core --startup-project .\\Palavyr.API --context AccountsContext --output "$accountsOutput\\Script$nextMigrationScriptVersion-accounts_migration-$name.sql" --idempotent
}
else {
    Write-Host "`r`nNot creating sql script for Accounts DB - no new migrations.";
}

if ($ConfigResult) {
    Write-Host "`r`nExporting Configuration Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $configOutput\\$nextMigrationScriptVersion-configuration_migration-$name.sql"
    dotnet ef migrations script --project .\\Palavyr.Core --startup-project .\\Palavyr.API --context DashContext --output "$configOutput\\Script$nextMigrationScriptVersion-configuration_migration-$name.sql" --idempotent
}
else {
    Write-Host "`r`nNotCreating sql script for Config DB - no new migrations."
}

if ($ConvoResult) {
    Write-Host "`r`nExporting Conversation Migrations as SQL Scripts..."
    Write-Host "`r`nExporting To: $convoOutput\\$nextMigrationScriptVersion-convo_migration-$name.sql"
    dotnet ef migrations script --project .\\Palavyr.Core --startup-project .\\Palavyr.API --context ConvoContext --output "$convoOutput\\Script$nextMigrationScriptVersion-convo_migration-$name.sql" --idempotent
}
else {
    Write-Host "`r`nNotCreating sql script for Config DB - no new migrations."
}

if ($supressmigration) {
    Write-Host "`r`nSkipping applying the migrations. You will need to run ./startMigrator.ps1 manually to apply these migrations. Finished."
}
else {
    Write-Host "`r`nMigrating the local database using DBUP!`r`n"
    & "$PSScriptRoot\\StartMigrator.ps1"
    Write-Host "`r`n`r`nAll set! You can check these new scripts into Git and deploy them with the server!`r`n"
}

