
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
  description = "AWS VPC CIDR range"
}

variable "hosted_zone_domain_name" {
  type = string
  description = "value of the Hosted Zone Domain Name"
}