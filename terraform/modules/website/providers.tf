terraform {
  required_version = ">= 1.0.8"
  required_providers {
    aws = {
      version = ">= 4.25.0"
      source  = "hashicorp/aws"
    }
  }
}

provider "aws" {
  region = var.aws_region
}