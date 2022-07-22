# Data Migrator app

For this to work, we just need two env vars set as such:

```csharp
Palavyr_ConnectionString=<YourConnectionString>`
Palavyr_Environment=Development\Production  (This one doesn't really matter :D)
```
private const string ConnectionStringAppSettingsKey = "ConnectionString";
private const string Environment = "Environment";


## Details

This migrator runs on aws lambda! So make you set your env vars there :D 

When building the function in terraform durin github action, just set the env vars there.
