$functionName = "${AWSFunctionName}-Migrator"
$functionRole = "arn:aws:iam::306885252482:role/service-role/PalavyrServerless"
$functionRunTime = "dotnetcore3.1"
$functionHandler = "Palavyr.Data.Migrator::Palavyr.Data.Migrator.MigratorLambdaHandler::MigratorHander"
$functionMemorySize = "512"
$functionDescription = "Migrator app for the palavyr server serverless RDS aurora postgres DB"
$regionName = "#{AWS:Region}"
$functionTimeout = "900"
$functionVPCSecurityGroupId = "sg-06541546adc6b38ca"
# $functionVPCSubnetId = "subnet-28107574"

$functionEnvironmentVariables = $OctopusParameters["AWS.Lambda.EnvironmentVariables"]
$functionEnvironmentVariablesKey = $OctopusParameters["AWS.Lambda.EnvironmentVariablesKey"]
$functionTags = $OctopusParameters["AWS.Lambda.Tags"]
$functionFileSystemConfig = $OctopusParameters["AWS.Lambda.FileSystemConfig"]
$functionDeadLetterConfig = $OctopusParameters["AWS.Lambda.DeadLetterConfig"]
$functionTracingConfig = $OctopusParameters["AWS.Lambda.TracingConfig"]


###########################################
############ VAR SUB ##############
###########################################
$newArchiveFileName = $OctopusParameters["Octopus.Action.Package[palavyr.server.windows].PackageFilePath"]



$tempDir = "templambda";
$packageFile = "palavyr.server.windows.zip"
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



###########################################
###########################################
###########################################



Write-Host "FUNCTION NAME --------: " + $functionName

if ([string]::IsNullOrWhiteSpace($functionName))
{
	Write-Error "The parameter Function Name is required."
    Exit 1
}

if ([string]::IsNullOrWhiteSpace($functionRole))
{
	Write-Error "The parameter Role is required."
    Exit 1
}

if ([string]::IsNullOrWhiteSpace($functionRunTime))
{
	Write-Error "The parameter Run Time is required."
    Exit 1
}

if ([string]::IsNullOrWhiteSpace($functionHandler))
{
	Write-Error "The parameter Handler is required."
    Exit 1
}

Write-Host "Function Name: $functionName"
Write-Host "Function Role: $functionRole"
Write-Host "Function Runtime: $functionRunTime"
Write-Host "Function Handler: $functionHandler"
Write-Host "Function Memory Size: $functionMemorySize"
Write-Host "Function Description: $functionDescription"
Write-Host "Function Subnet Ids: $functionVPCSubnetId"
Write-Host "Function Security Group Ids: $functionVPCSecurityGroupId"
Write-Host "Function Environment Variables: $functionEnvironmentVariables"
Write-Host "Function Environment Variables Key: $functionEnvironmentVariablesKey"
Write-Host "Function Timeout: $functionTimeout"
Write-Host "Function Tags: $functionTags"
Write-Host "Function File System Config: $functionFileSystemConfig"
Write-Host "Function Dead Letter Config: $functionDeadLetterConfig"
Write-Host "Function Tracing Config: $functionTracingConfig"
Write-Host "Function file path: fileb://$newArchiveFileName"

Write-Host "Attempting to find the function $functionName in the region $regionName"
$hasExistingFunction = $true
try
{
	aws lambda get-function --function-name "$functionName"
    if ($LASTEXITCODE -eq 255)
    {
    	Write-Host "The function was not found and an exit code for 255 was returned"
    	$hasExistingFunction = $false
    }
}
catch
{
	Write-Host "The function was not found"
	$hasExistingFunction = $false
}

Write-Host "Existing functions: $hasExistingFunction"

if ($hasExistingFunction -eq $false)
{
	Write-Highlight "Creating $functionName in $regionName"
	aws lambda create-function --function-name $functionName --zip-file fileb://$newArchiveFileName --handler $functionHandler --runtime $functionRuntime --role $functionRole --memory-size $functionMemorySize
}
else
{
	Write-Highlight "Updating the $functionName code"
    aws lambda update-function-code --function-name $functionName --zip-file fileb://$newArchiveFileName

    Write-Highlight "Updating the $functionName base configuration"
    aws lambda update-function-configuration --function-name $functionName --role $functionRole --handler $functionHandler --runtime $functionRuntime --memory-size $functionMemorySize
}

if ([string]::IsNullOrWhiteSpace($functionEnvironmentVariables) -eq $false)
{
	Write-Highlight "Environment variables specified, updating environment variables configuration for $functionName"
	$environmentVariables = "Variables={$functionEnvironmentVariables}"

    if ([string]::IsNullOrWhiteSpace($functionEnvironmentVariablesKey) -eq $true)
    {
    	aws lambda update-function-configuration --function-name $functionName --environment "$environmentVariables"
    }
    else
    {
    	aws lambda update-function-configuration --function-name $functionName --environment "$environmentVariables" --kms-key-arn "$functionEnvironmentVariablesKey"
    }
}

if ([string]::IsNullOrWhiteSpace($functionTimeout) -eq $false)
{
	Write-Highlight "Timeout specified, updating timeout configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --timeout "$functionTimeout"
}

if ([string]::IsNullOrWhiteSpace($functionTags) -eq $false)
{
	Write-Highlight "Tags specified, updating tags configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --tags "$functionTags"
}

if ([string]::IsNullOrWhiteSpace($functionVPCSubnetId) -eq $false -and [string]::IsNullOrWhiteSpace($functionVPCSecurityGroupId) -eq $false)
{
	Write-Highlight "VPC subnets and security group specified, updating vpc configuration for $functionName"
	$vpcConfig = "SubnetIds=$functionVPCSubnetId,SecurityGroupIds=$functionVPCSecurityGroupId"
	aws lambda update-function-configuration --function-name $functionName --vpc-config "$vpcConfig"
}

if ([string]::IsNullOrWhiteSpace($functionDescription) -eq $false)
{
	Write-Highlight "Description specified, updating description configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --description "$functionDescription"
}

if ([string]::IsNullOrWhiteSpace($functionFileSystemConfig) -eq $false)
{
	Write-Highlight "File System Config specified, updating file system configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --file-system-configs "$functionFileSystemConfig"
}

if ([string]::IsNullOrWhiteSpace($functionDeadLetterConfig) -eq $false)
{
	Write-Highlight "Description specified, updating description configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --dead-letter-config "$functionDeadLetterConfig"
}

if ([string]::IsNullOrWhiteSpace($functionTracingConfig) -eq $false)
{
	Write-Highlight "Description specified, updating description configuration for $functionName"
	aws lambda update-function-configuration --function-name $functionName --tracing-config "$functionTracingConfig"
}

Write-Highlight "AWS Lambda $functionName successfully deployed."