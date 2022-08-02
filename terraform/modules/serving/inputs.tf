
variable "environment" {
  type        = string
  description = "The environment to use (dev, staging, production)"
}

variable "region" {
  type        = string
  description = "The AWS Region to use"
  default     = "us-east-1"
}

variable "domain_name" {
  type        = string
  description = "The domain name to use"
}

variable "vpc_cidr" {
  type        = string
  default     = "10.10.0.0/16"
  description = "AWS VPC CIDR range"
}
