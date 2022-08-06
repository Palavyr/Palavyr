variable "database_name" {
  type        = string
  description = "name of the database"
}

variable "database_username" {
  type = string
  description "username for the database"
}

variable "instance_class" {
  type        = string
  description = "instance class to use - .e.g. db.t3.micro"
}

variable "public_subnets" {
  type        = list(any)
  description = "list of subnets to use for the database"
}

variable "rds_param_group_name" {
  type        = string
  description = "name of the parameter group"
}

variable "protect_from_deletion" {
  type        = bool
  description = "whether to prevent deletion of this resource"
}
variable "aws_region" {
  type        = string
  description = "aws region"
}

