param ([string]$workspacename)
# https://docs.microsoft.com/en-us/azure/developer/terraform/store-state-in-azure-storage?tabs=azure-cli

$help = @"
    Setup Script

    Usage: ./setup.ps1 -w [your-unique-workspace]

    [options]

    -h, --help                 Show this help message and exit
    -w, --workspace            The workspace to use

    Required:

        You MUST set two environment variables:
            - a terraform cloud api token

                TF_TOKEN_app_terraform_io=<your-api-key>

            - the sandbox subscription id

                CorePlatformSandboxSubId=<Core-Platform-Sandbox-Subscription-Id>

    Description:
    This script is used to set up your local dev environment for testing out infrastructure in development before you deploy it. It will create a workspace for you in terraform cloud (give your parameter), then it will
"@;


if ("" -eq $workspacename) {
    Write-Error "You must provide a workspace name"
    Write-Information $help;
    exit 1
}

if ("" -eq $env:TF_TOKEN_app_terraform_io) {
    Write-Error "You must set the terraform cloud api token"
    Write-Error "TF_TOKEN_app_terraform_io=<your-token>"
    Write-Information $help;
    exit 1
}

if ("" -eq $env:CorePlatformSandboxSubId) {
    Write-Error "You must set the sandbox subscription id"
    Write-Error "CorePlatformSandboxSubId=<subscription-id>"
    Write-Information $help;
    exit 1
}

Write-Host 'Creating new workspace: $workspacename'
terraform workspace new $workspacename
terraform workspace select $workspacename
# try {

#     Invoke-WebRequest "https://app.terraform.io/api/v2/organizations/octopus-deploy/workspaces" `
#         -Method POST `
#         -Headers @{'Content-Type' = 'application/vnd.api+json'; 'Accept' = 'application/json'; "Authorization" = "Bearer $env:TF_TOKEN_app_terraform_io" } `
#         -Body '{"data":{"attributes":{"name":"'$workspacename'","execution-mode":"local"}},"type":"workspaces"}'
# }
# catch {
#     Write-Error $_.Exception.Message
#     exit 1
# }

# terraform init

# # Do a round of checks to make sure everything is running correctly
# terraform validate
# terraform apply --var-file .\envs\other.tfvars
# terraform destroy -var-file .\envs\other.tfvars

Write-Host "Congradulations, you're environment is all set up!"