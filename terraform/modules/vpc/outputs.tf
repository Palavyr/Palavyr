output "vpc_id" {
  # type        = string
  # description = "The VPC ID to use"
  value = module.vpc.vpc_id
}

output "vpc_private_subnets" {
  value = module.vpc.private_subnets
}
output "vpc_public_subnets" {
  value = module.vpc.public_subnets
}
