## Start the Palavyr.Data.Migrator console app in deveopment mode

param([string]$environment = "Development")

$env:ASPNETCORE_ENVIRONMENT = $environment

$env:Palavyr_DB__Host = "localhost"
$env:Palavyr_DB__Port = "5432"
$env:Palavyr_DB__DbName = "AppDatabase"
$env:Palavyr_DB__Username = "postgres"
$env:Palavyr_DB__Password = "Password01!"


dotnet clean
dotnet build

$databaseProcess = Start-Process Palavyr.Data.Migrator\bin\Debug\net6.0\Palavyr.Data.Migrator.exe -PassThru -Wait -NoNewWindow

exit $databaseProcess.ExitCode
