dotnet ef migrations script --project Palavyr.API --context AccountsContext --output accounts_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent
dotnet ef migrations script --project Palavyr.API --context DashContext --output configuration_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent
dotnet ef migrations script --project Palavyr.API --context ConvoContext --output convo_migration.%dep.ConfigurationManager_CreateVersionNumber.system.build.number%.sql --idempotent