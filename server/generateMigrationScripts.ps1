try {
    dotnet tool install --global dotnet-ef --version=3.1.0
}
catch {
    Write-Host "Tool already installed" -ForegroundColor RED
}

dotnet ef migrations script --project ./server/Palavyr.API --context AccountsContext --output accounts_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent
dotnet ef migrations script --project ./server/Palavyr.API --context DashContext --output configuration_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent
dotnet ef migrations script --project ./server/Palavyr.API --context ConvoContext --output convo_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent