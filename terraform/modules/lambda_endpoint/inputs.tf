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
  type = string
  description  = "name of the lambda function"
}