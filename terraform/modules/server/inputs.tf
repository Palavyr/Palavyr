#  Application load balancer inputs

variable "application_load_balancer_name" {
  type        = string
  description = "The name for the application load blancer"
}

variable "public_subnets" {
  type        = list(string)
  description = "use module.vpc.public_subnets"
}

# AutoScale Group inputs
variable "instance_type" {
  type        = string
  description = "e.g. use t2.micro"
}

variable "aws_account_id" {
  type        = string
  description = "The AWS account ID for ECR"
}

variable "autoscale_group_name" {
  type        = string
  description = "The name for the autoscale group"
}

variable "private_subnets" {
  type        = list(string)
  description = "use module.vpc.private_subnets"
}

variable "ecr_access_key" {
  type        = string
  description = "The AWS access key for ECR"
}

variable "ecr_secret_key" {
  type        = string
  description = "The AWS secret key for ECR"
}


# Domain name
variable "domain_name" {
  type        = string
  description = "The domain name to use"
}

variable "hosted_zone_domain_name" {
  type        = string
  description = "value of the Hosted Zone Domain Name"
}

# Common
variable "vpc_id" {
  type        = string
  description = "The VPC ID to use"
}


variable "aws_region" {
  type        = string
  description = "The AWS Region to use"
  default     = "us-east-1"
}

variable "tags" {
  type        = any
  description = "The tags to use"
}


variable "environment" {
  type        = string
  description = "The environment in use"
}

variable "octopus_api_key" {
  type = string
}

variable "role" {
  type        = string
  description = "The role which will be used when deploying to autoscale"
}
