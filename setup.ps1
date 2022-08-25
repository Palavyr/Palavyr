Write-Output ""
Write-Output "Welcome to the basic setup script"
Write-Output ""

Write-Output ""
Write-Output "Make sure you've set up your two .env files - on in the root of the project"
Write-Output ""

# Log in to the ECR registry
# Please ensure you've populated your local .env file
# If you haven't, make a copy of the .env.Template file and populate it


Get-Content .env.local | ForEach-Object {
    $name, $value = $_.split('=')
    if(string.$name )
    Set-Content env:\$name $value
}

Write-Output "Grabbing your AWS Elastic Container Registry credentials... assuming you've set up your .env file"
aws ecr get-login-password --region $env:Palavyr_AWS__Region | docker login --username AWS --password-stdin $env:ECR_REGISTRY
Write-Output ""


Write-Output "Composing your docker environment..."
docker compose -f ./docker-compose.yml up -d --remove-orphans
Write-Output ""

Write-Output "Moving to the utilities directory"
Set-Location ./server
Write-Output ""

Write-Output "Applying migrations..."
./utilities/StartMigrator.ps1
Write-Output ""

Set-Location ..
Write-Output "You're good to go start the app now."
