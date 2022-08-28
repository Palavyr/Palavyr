## Start the Palavyr.Data.Migrator console app in deveopment mode

param([string]$environment = "Development")

$env:ASPNETCORE_ENVIRONMENT = $environment

$env:Palavyr_DB__Host = "localhost"
$env:Palavyr_DB__Port = "5432"
$env:Palavyr_DB__DbName = "AppDatabase"
$env:Palavyr_DB__Username = "postgres"
$env:Palavyr_DB__Password = "Password01!"

# https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings
dotnet clean --verbosity q
dotnet build --verbosity q -noConsoleLogger


$databaseProcess = Start-Process Palavyr.Data.Migrator\bin\Debug\net6.0\Palavyr.Data.Migrator.exe -PassThru -Wait -NoNewWindow

exit $databaseProcess.ExitCode
