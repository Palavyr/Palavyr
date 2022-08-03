variable "environment" {
  type        = string
  description = "The environment to deploy to"
}

variable "lambda_runtime" {
  type        = string
  description = "The  lambda runtime to use"
}

variable "lambda_handler_name" {
  type        = string
  description = "Name of the lambda handler function"
}

variable "function_name" {
  type        = string
  description = "name of the lambda function"
}

variable "aws_iam_role_name" {
  type        = string
  description = "name of the IAM role"
}

variable "image_uri" {
  type        = string
  description = "image uri of the lambda container"
}

variable "region" {
  type        = string
  description = "region of the lambda function"
}

variable "gateway_name" {
  type        = string
  description = "name of the api gateway"
}

variable "gateway_stage_name" {
  type        = string
  description = "name of the api gateway stage - this is the suffix "
}
