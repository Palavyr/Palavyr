#######################

#  The initial block here defines this as a terraform file
#  When this gets deployed, the first block with the backend "remote"
#  code needs to be uncommented.


#  Below, where it says "For dev time" - you can use this for local tracking if you wish
#  Comment out remote terraform block, and uncomment the local block

##################
terraform {
  backend "remote" {
    organization = "palavyr"
    token        = "#{TerraformCloudApiToken}"

    workspaces {
      name = "#{TerraformWorkspace}"
    }
  }

  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}

#######
# Uncomment the code below when working locally if you want to track your
# own infra while testing in development
#######

# terraform {
#   backend "local" {
#   }
#   required_providers {
#     aws = {
#       source  = "hashicorp/aws"
#       version = "~> 4.0"
#     }
#   }
# }

#################################################################
provider "aws" {
  alias  = "aws"
  region = var.aws_region
}


####################################################################

resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "OAI for ${var.hosted_zone_domain_name}"
}

locals {
  tags = {
    "Project"   = "Palavyr-${var.environment}"
    "ManagedBy" = "Terraform"
  }
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

module "vpc" {
  source = "./modules/vpc"

  aws_region = var.aws_region
  vpc_cidr   = "10.10.0.0/16"
  vpc_name   = "palavyr-vpc-${lower(var.environment)}"
  tags       = local.tags
}

module "server_group" {
  source = "./modules/server"

  application_load_balancer_name = "palavyr-load-balancer-${lower(var.environment)}"
  autoscale_group_name           = "palavyr-autoscale-group-${lower(var.environment)}"
  public_subnets                 = module.vpc.public_subnets
  private_subnets                = module.vpc.private_subnets
  vpc_id                         = module.vpc.vpc_id
  security_group_id              = module.vpc.security_group_id
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

  tags = local.tags
}

module "database" {
  source = "./modules/database"

  database_name              = "palavyr-${lower(var.environment)}"
  database_username          = "palavyruser${lower(var.environment)}"
  rds_param_group_name       = "palavyr-rds-param-group-${lower(var.environment)}"
  database_subnet_group_name = "palavyr-rds-subnet-group-${lower(var.environment)}"
  instance_class             = var.database_instance_type
  protect_from_deletion      = var.protect_from_deletion
  aws_region                 = var.aws_region
  db_password                = var.database_password
  public_subnets             = module.vpc.public_subnets
  security_group_id          = module.vpc.security_group_id

  tags = local.tags
}


module "pdf_server" {
  source = "./modules/lambda_endpoint"

  environment        = var.environment
  function_name      = "palavyr-pdf-server-${lower(var.environment)}"
  aws_iam_role_name  = "serverless_lambda_${var.environment}"
  aws_region         = var.aws_region
  gateway_name       = "api-gateway-pdf-server-${lower(var.environment)}"
  gateway_stage_name = lower(var.environment)
  image_uri          = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/palavyr/palavyr-pdf-server-lambda:latest"
  tags               = local.tags
}


resource "random_id" "randid" {
  byte_length = 6
}

module "palavyr_user_data_bucket" {
  source = "./modules/data_buckets"

  bucket_name           = "palavyr-user-data-${lower(var.environment)}"
  protect_from_deletion = var.protect_from_deletion
  key_alias             = "alias/palavyr_${random_id.randid.hex}_key_${lower(var.environment)}"
  tags                  = local.tags
}
