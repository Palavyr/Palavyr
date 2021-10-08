
$tempDir = "templambda";
$packageFile = "AWS.Lambda.Package.zip"
$unpackedSettings = ".\${tempDir}\appsettings.migrator.json"
# unzip the package
Expand-Archive $packageFile -DestinationPath $tempDir -Force
$config = Get-Content $unpackedSettings | Out-String | ConvertFrom-Json
#########################################################################


$config.ConnectionStrings.'AccountsContextPostgres' = '#{ConnectionStrings:AccountsContextPostgres}'
$config.ConnectionStrings.'ConvoContextPostgres' = '#{ConnectionStrings:ConvoContextPostgres}'
$config.ConnectionStrings.'DashContextPostgres' = '#{ConnectionStrings:DashContextPostgres}'
$config.MigratorEnv.'Environment' = '#{MigratorEnv:Environment}'


##########################################################################

# Write config back to appsettings.json

ConvertTo-Json $config -Depth 100 | Out-File $unpackedSettings -Force

# Rezip the lambda package

Compress-Archive "${tempDir}/*" -DestinationPath $packageFile -Force

Remove-Item "${tempDir}" -Force -Recurse

