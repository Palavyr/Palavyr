variable "aws_region" {
  type        = string
  description = "The AWS Region to use"
  default     = "us-east-1"
}

variable "bucket_prefix" {
  type        = string
  description = "The prefix for the S3 bucket"
}

variable "site_domain_name" {
  type        = string
  description = "The domain name to use for the app"
}

variable "environment" {
  type        = string
  description = "The environment to use (dev, staging, production)"
}

variable "hosted_zone_domain_name" {
  type        = string
  description = "The hosted zone domain name"
}

variable "cloudfront_access_id_path" {
  type        = string
  description = "cloudfront_access_id_path"
}

variable "cloudfront_access_id_iam_arn" {
  type        = string
  description = "cloudfront_access_id_iam_arn"
}
