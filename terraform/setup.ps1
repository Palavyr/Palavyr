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

                TERRAFORM_API_KEY=<your-api-key>

            - the sandbox subscription id

                CorePlatformSandboxSubId=<Core-Platform-Sandbox-Subscription-Id>

    Description:
    This script is used to set up your local dev environment for testing out infrastructure in development before you deploy it. It will create a workspace for you in terraform cloud (give your parameter), then it will
"@;


# if ("" -eq $workspacename) {
#     Write-Error "You must provide a workspace name"
#     Write-Information $help;
#     exit 1
# }

if ("" -eq $env:TERRAFORM_API_KEY) {
    Write-Error "You must set the terraform cloud api token"
    Write-Error "TERRAFORM_API_KEY=<your-token>"
    Write-Information $help;
    exit 1
}

if ("" -eq $env:CorePlatformSandboxSubId) {
    Write-Error "You must set the sandbox subscription id"
    Write-Error "CorePlatformSandboxSubId=<subscription-id>"
    Write-Information $help;
    exit 1
}

# Write-Host 'Creating new workspace: $workspacename'
# try {

#     Invoke-WebRequest "https://app.terraform.io/api/v2/organizations/octopus-deploy/workspaces" `
#         -Method POST `
#         -Headers @{'Content-Type' = 'application/vnd.api+json'; 'Accept' = 'application/json'; "Authorization" = "Bearer $env:TERRAFORM_API_KEY" } `
#         -Body '{"data":{"attributes":{"name":"'$workspacename'","execution-mode":"local"}},"type":"workspaces"}'
# }
# catch {
#     Write-Error $_.Exception.Message
#     exit 1
# }

$API_KEY = $env:TERRAFORM_API_KEY;
Write-Host $API_KEY

(Get-Content ./backend.tf).replace('#{TERRAFORM_WORKSPACE}', 'palavyr-local-dev') | Set-Content ./backend.tf
(Get-Content ./backend.tf).replace('#{TERRAFORM_API_KEY}', $API_KEY) | Set-Content ./backend.tf

terraform init

# Do a round of checks to make sure everything is running correctly
terraform validate
terraform apply --var-file .\envs\dev.tfvars
terraform destroy -var-file .\envs\dev.tfvars

(Get-Content ./backend.tf).replace('palavyr-local-dev', '#{TERRAFORM_WORKSPACE}') | Set-Content ./backend.tf
(Get-Content ./backend.tf).replace( $API_KEY, '#{TERRAFORM_API_KEY}') | Set-Content ./backend.tf

Write-Host "Congradulations, you're environment is all set up!"