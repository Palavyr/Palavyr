# Log in to the ECR registry
# Please ensure you've populated your local .env file
# If you haven't, make a copy of the .env.Template file and populate it

Get-Content .env | ForEach-Object {
    $name, $value = $_.split('=')
    Set-Content env:\$name $value
}

aws ecr get-login-password --region $env:Palavyr_AWS__Region | docker login --username AWS --password-stdin $env:ECR_REGISTRY