variable "instance_region" {
  type        = string
  description = "Region for the instance"
}

variable "subnet_id_1_private" {
  type        = string
  description = "subnet id to place the instance on"
}
variable "subnet_id_2_private" {
  type        = string
  description = "subnet id to place the instance on"
}
variable "subnet_id_3_public" {
  type        = string
  description = "subnet id to place the instance on"
}

variable "instance_name" {
  type        = string
  description = "Name of the instance"
}

variable "security_group_ids" {
  type        = list(string)
  description = "Security group ids to place the instance in"
}

variable "vpc_id" {
  type = string
  description = "vpc id"
}