# Notes:
# https://docs.microsoft.com/en-us/azure/developer/terraform/store-state-in-azure-storage?tabs=azure-cli

terraform {
  cloud {
    organization = "palavyr"
    hostname     = "app.terraform.io"

    workspaces {
      name = "palavyr-local-dev"
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
