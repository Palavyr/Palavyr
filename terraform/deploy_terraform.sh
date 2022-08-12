#!bin/bash

TerraformApiKey=$1
Workspace=$2

help="A script for running terraform deployments in CI (octopus deploy)"

case "${1}" in
    '') echo "You must provide a terraform cloud api key\n\n${help}"; exit 1;;
    esac

case "${2}" in
    '') echo "You must provide a workspace name\n\n${help}"; exit 1;;
    esac


TF_WORKSPACE=#{TF_WORKSPACE} TF_TOKEN_app_terraform_io=#{TerraformApiKey} AWS_ACCESS_KEY_ID=#{AWS_PALAVYR_ACCESS_KEY} AWS_SECRET_ACCESS_KEY=#{AWS_PALAVYR_SECRET_KEY} AWS_DEFAULT_REGION=#{AWS_REGION}

TF_WORKSPACE=#{TF_WORKSPACE} TF_TOKEN_app_terraform_io=#{TerraformApiKey} AWS_ACCESS_KEY_ID=#{AWS_PALAVYR_ACCESS_KEY} AWS_SECRET_ACCESS_KEY=#{AWS_PALAVYR_SECRET_KEY} AWS_DEFAULT_REGION=#{AWS_REGION} terraform init
TF_WORKSPACE=#{TF_WORKSPACE} TF_TOKEN_app_terraform_io=#{TerraformApiKey} AWS_ACCESS_KEY_ID=#{AWS_PALAVYR_ACCESS_KEY} AWS_SECRET_ACCESS_KEY=#{AWS_PALAVYR_SECRET_KEY} AWS_DEFAULT_REGION=#{AWS_REGION} terraform apply --var-file ./envs/deployment.tfvars