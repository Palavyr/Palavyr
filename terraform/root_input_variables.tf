# Inputs. These must all be specified in the envs/{stage}.tfvars

variable "aws_region" {
  description = "AWS region for all resources. e.g. us-east-1"
  type        = string
}

variable "environment" {
  description = "Deployment environment"
  type        = string
}

variable "hosted_zone_domain_name" {
  description = "The hosted zone domain name"
  type        = string
}

variable "app_domain_name" {
  description = "Domain name for the configuration application"
  type        = string
}

variable "widget_domain_name" {
  description = "Domain name for the widget application"
  type        = string
}

variable "server_domain_name" {
  description = "The domain name for the server"
  type        = string
}

variable "protect_from_deletion" {
  description = "whether or not to protect from deletion"
  type        = bool
}

variable "aws_account_id" {
  description = "value to determine the ECR AWS account ID"
  type        = string
}

variable "database_instance_type" {
  description = "The type of instance that runs the database"
  type        = string
}

variable "scale_group_instance_type" {
  description = "the type of instance that runs the main server"
  type        = string
}

variable "ecr_secret_key" {
  type = string
}

variable "ecr_access_key" {
  type = string
}

variable "database_password" {
  type        = string
  description = "password to connect to the RDS postgres database - this is stored in Octopus and Lastpass"
}

variable "octopus_api_key" {
  type = string
}

variable "role" {
  type = string
}
