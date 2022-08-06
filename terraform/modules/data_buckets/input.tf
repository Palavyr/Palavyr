variable "bucket_name" {
  type        = string
  description = "bucket name"
}

variable "protect_from_deletion" {
  type        = bool
  description = "whether or not to protect from deletion"
}

variable "tags" {
  type = any
}
