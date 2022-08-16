resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "OAI for ${var.hosted_zone_domain_name}"
}

module "configuration_app_website" {
  source                       = "./modules/website"
  aws_region                   = var.aws_region
  site_domain_name             = var.app_domain_name
  environment                  = var.environment
  hosted_zone_domain_name      = var.hosted_zone_domain_name
  cloudfront_access_id_path    = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
  cloudfront_access_id_iam_arn = aws_cloudfront_origin_access_identity.oai.iam_arn
  tags                         = local.tags
}

module "widget_app_website" {
  source                       = "./modules/website"
  aws_region                   = var.aws_region
  site_domain_name             = var.widget_domain_name
  environment                  = var.environment
  hosted_zone_domain_name      = var.hosted_zone_domain_name
  cloudfront_access_id_path    = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
  cloudfront_access_id_iam_arn = aws_cloudfront_origin_access_identity.oai.iam_arn
  tags                         = local.tags
}


locals {
  vpc_cidr = "10.10.0.0/16"
}

module "vpc" {
  source  = "terraform-aws-modules/vpc/aws"
  version = "2.77.0"

  name = "palavyr-vpc-${random_id.rand.hex}-${lower(var.environment)}"
  cidr = local.vpc_cidr

  azs = ["${var.aws_region}a", "${var.aws_region}b"]
  public_subnets = [
    cidrsubnet(local.vpc_cidr, 8, 0),
    cidrsubnet(local.vpc_cidr, 8, 1)
  ]
  private_subnets = [
    cidrsubnet(local.vpc_cidr, 8, 2),
    cidrsubnet(local.vpc_cidr, 8, 3)
  ]

  enable_nat_gateway   = true
  single_nat_gateway   = true
  enable_dns_hostnames = true

  tags = local.tags

}


resource "random_id" "server_id" {
  byte_length = 3
}

module "server_group" {
  source = "./modules/server"

  application_load_balancer_name = "palavyr-lb-${random_id.server_id.hex}-${lower(var.environment)}"
  autoscale_group_name           = "palavyr-ag-${random_id.server_id.hex}-${lower(var.environment)}"
  public_subnets                 = module.vpc.public_subnets
  private_subnets                = module.vpc.private_subnets
  vpc_id                         = module.vpc.vpc_id
  instance_type                  = var.scale_group_instance_type
  domain_name                    = var.server_domain_name
  hosted_zone_domain_name        = var.hosted_zone_domain_name
  aws_region                     = var.aws_region
  aws_account_id                 = var.aws_account_id
  ecr_access_key                 = var.ecr_access_key
  ecr_secret_key                 = var.ecr_secret_key
  environment                    = var.environment
  octopus_api_key                = var.octopus_api_key
  role                           = "palavyr-autoscale"
  tags                           = local.tags
}

module "database" {
  source = "./modules/database"

  database_name              = "palavyr-${lower(var.environment)}-${random_id.rand.hex}"
  database_username          = "palavyr"
  rds_param_group_name       = "palavyr-rds-pgroup-${lower(var.environment)}-${random_id.rand.hex}"
  database_subnet_group_name = "palavyr-rds-sgroup-${lower(var.environment)}-${random_id.rand.hex}"
  instance_class             = var.database_instance_type
  protect_from_deletion      = var.protect_from_deletion
  aws_region                 = var.aws_region
  db_password                = var.database_password
  public_subnets             = module.vpc.private_subnets
  vpc_id                     = module.vpc.vpc_id

  tags = local.tags
}


module "pdf_server" {
  source = "./modules/lambda_endpoint"

  environment        = var.environment
  function_name      = "palavyr-pdf-server-${lower(var.environment)}-${random_id.rand.hex}"
  aws_iam_role_name  = "pdflambda-${var.environment}-${random_id.rand.hex}"
  aws_region         = var.aws_region
  gateway_name       = "agw-pdf-server-${lower(var.environment)}-${random_id.rand.hex}"
  gateway_stage_name = lower(var.environment)
  image_uri          = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/palavyr/palavyr-pdf-server-lambda:latest"
  tags               = local.tags
}



module "palavyr_user_data_bucket" {
  source                = "./modules/data_buckets"
  bucket_name           = "palavyr-user-data-${lower(var.environment)}-${random_id.rand.hex}"
  protect_from_deletion = var.protect_from_deletion
  key_alias             = "alias/palavyr_key_${lower(var.environment)}-${random_id.rand.hex}"
  tags                  = local.tags
}
