
terraform {
  required_providers {
    aws = {
      version = "~> 4.24.0"
    }
  }
}

provider "aws" {
  region = "us-east-1"
}
