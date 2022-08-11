# https://docs.microsoft.com/en-us/azure/developer/terraform/store-state-in-azure-storage?tabs=azure-cli

$help = "
Setup Script

Usage: ./setup.ps1 -w [your-unique-workspace]

[options]

-h, --help                 Show this help message and exit
-w, --workspace            The workspace to use

Required:

    You MUST set a terraform cloud api token in your environment variables.

    TF_TOKEN_app_terraform_io=<your-api-key>


Description:
This script is used to set up your local dev environment for testing out infrastructure in development before you deploy it. It will create a workspace for you in terraform cloud (give your parameter), then it will
"

workspacename=$1

case "${1}" in
    '') echo "You must provide a workspace name\n\n${help}"; exit 1;;
    esac

case "${TF_TOKEN_app_terraform_io}" in
    '') echo "You must set the terraform cloud api token.\n\nTF_TOKEN_app_terraform_io=<your-token>\n\n ${help}"; exit 1;;
    esac

case "${CorePlatformSandboxSubId}" in
    '') echo "You must set the sandbox subscription id.\n\nTF_TOKEN_app_terraform_io=<your-token>\n\n ${help}"; exit 1;;
    esac

echo "\nCreating new workspace: $workspacename"\n


curl \
  --fail --silent --show-error \
  --header "Authorization: Bearer $TF_TOKEN_app_terraform_io" \
  --header "Content-Type: application/vnd.api+json" \
  --request POST \
  --data '{"data":{"attributes":{"name":"'$workspacename'","execution-mode":"local"}},"type":"workspaces"}' \
  https://app.terraform.io/api/v2/organizations/octopus-deploy/workspaces



az login
az account set --subscription $env:CorePlatformSandboxSubId
terraform init

echo "\n\nDo a round of checks to make sure everything is running correctly\n\n"
terraform validate
terraform apply --var-file ".\envs\dev\dev.tfvars" -auto-approve
terraform destroy -var-file ".\envs\dev\dev.tfvars" -auto-approve

echo "\nCongradulations, you're environment is all set up!\n"

