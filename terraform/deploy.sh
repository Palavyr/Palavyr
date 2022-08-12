!#bin/bash

TerraformApiKey=$1
Workspace=$2

help="A script for running terraform deployments in CI (octopus deploy)"

case "${1}" in
    '') echo "You must provide a terraform cloud api key\n\n${help}"; exit 1;;
    esac

case "${2}" in
    '') echo "You must provide a workspace name\n\n${help}"; exit 1;;
    esac


export TF_WORKSPACE="${Workspace}"
export TF_TOKEN_app_terraform_io="${TerraformApiKey}"

cat ./envs/deployment.tfvars
terraform init
terraform apply --var-file ./envs/deployment.tfvars