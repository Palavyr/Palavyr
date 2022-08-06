variable "vpc_name" {
  type        = string
  description = "name for the vpc"
}


variable "vpc_cidr" {
  type        = string
  description = "AWS VPC CIDR range"
}

variable "aws_region" {
  type        = string
  description = "The AWS Region to use"
}

variable "tags" {
  type        = any
  description = "The tags to use"
}
