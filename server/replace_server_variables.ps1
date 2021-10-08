
$tempDir = "templambda";
$packageFile = "AWS.Lambda.Package.zip"
$unpackedSettings = ".\${tempDir}\appsettings.json"
# unzip the package
Expand-Archive $packageFile -DestinationPath $tempDir -Force
$config = Get-Content $unpackedSettings | Out-String | ConvertFrom-Json
#########################################################################

$config.Logging.LogLevel."Default" = "#{Logging:LogLevel:Default}"
$config.Logging.LogLevel."Microsoft" = "#{Logging:LogLevel:Microsoft}"
$config.Logging.LogLevel."Microsoft.Hosting.Lifetime" = "#{Logging:LogLevel:Microsoft.Hosting.Lifetime}"
$config.Logging.LogLevel."Microsoft.EntityFrameworkCore.Database.Command" = "#{Microsoft.EntityFrameworkCore.Database.Command}"
$config.ConnectionStrings."AccountsContextPostgres" = "#{ConnectionStrings:AccountsContextPostgres}"
$config.ConnectionStrings."ConvoContextPostgres" = "#{ConnectionStrings:ConvoContextPostgres}"
$config.ConnectionStrings."DashContextPostgres" = "#{ConnectionStrings:DashContextPostgres}"
$config.Postgres."host" = "#{Postgres:host}"
$config.Postgres."port" = "#{Postgres:port}"
$config.Postgres."password" = "#{Postgres:password}"
$config.AWS."PostgresPassword" = "#{AWS:PostgresPassword}"
$config.AWS."PostgresUsername" = "#{AWS:PostgresUsername}"
$config.AWS."PostgresPort" = "#{AWS:PostgresPort}"
$config.AWS."PostgresEndpoint" = "#{AWS:PostgresEndpoint}"
$config.AWS."SecretKey" = "#{AWS:SecretKey}"
$config.AWS."AccessKey" = "#{AWS:AccessKey}"
$config.AWS."Region" = "#{AWS:Region}"
$config.AWS."ProfilesLocation" = "#{AWS:ProfilesLocation}"
$config.Stripe."SecretKey" = "#{Stripe:SecretKey}"
$config.Stripe."WebhookKey" = "#{Stripe:WebhookKey}"
$config.JWTSecretKey = "#{JWTSecretKey}"
$config.Backups = "#{Backups}"
$config.Previews = "#{Previews}"
$config.Userdata = "#{Userdata}"

# NOT USED CURRENTLY
# $config.jwtTokenConfig."secret" = "#{}"
# $config.jwtTokenConfig."issuer" = "#{}"
# $config.jwtTokenConfig."audience" = "#{}"
# $config.jwtTokenConfig."accessTokenExpiration" = "#{}"
# $config.jwtTokenConfig."refreshTokenExpiration" = "#{}"
$config."Pdf.Server.Port" = "#{Pdf.Server.Port}"
$config."Pdf.Server.Host" = "#{Pdf.Server.Host}"
$config."Palavyr.Server.Environment" = "#{Palavyr.Server.Environment}"

##########################################################################


Write-Host "DID THIS WORK ---------------------"

Write-Host $config."Palavyr.Server.Environment"

# Write config back to appsettings.json

ConvertTo-Json $config -Depth 100 | Out-File $unpackedSettings -Force

# Rezip the lambda package

Compress-Archive "${tempDir}/*" -DestinationPath $packageFile -Force

Remove-Item "${tempDir}" -Force -Recurse