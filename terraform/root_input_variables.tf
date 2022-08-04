# Input variable definitions
variable "aws_region" {
  description = "AWS region for all resources."
  type        = string
  default     = "us-east-1"
}

variable "environment" {
  description = "Deployment environment"
  type        = string
}

variable "hosted_zone_domain_name" {
  type        = string
  description = "The hosted zone domain name"
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
  type        = string
  description = "The domain name for the server"
}

variable "can_destroy_data_buckets" {
  type        = bool
  description = "value to determine if data buckets can be destroyed"
}

variable "aws_account_id" {
  type        = string
  description = "value to determine the ECR AWS account ID"
}
