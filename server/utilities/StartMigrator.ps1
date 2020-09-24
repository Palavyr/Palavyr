## Start the Palavyr.Data.Migrator console app in deveopment mode

param([string]$environment = "Development")

$env:ASPNETCORE_ENVIRONMENT = $environment
$databaseProcess = Start-Process Palavyr.Data.Migrator\bin\Debug\netcoreapp3.1\Palavyr.Data.Migrator.exe -PassThru -Wait -NoNewWindow

exit $databaseProcess.ExitCode

