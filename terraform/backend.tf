# Notes:
# https://docs.microsoft.com/en-us/azure/developer/terraform/store-state-in-azure-storage?tabs=azure-cli

terraform {
  cloud {
    organization = "palavyr"
    hostname     = "app.terraform.io"
    token = "#{TERRAFORM_API_KEY}"
    workspaces {
      name = "#{TERRAFORM_WORKSPACE}"

    }
  }

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

provider "aws" {
  region = var.aws_region
}


























































































































