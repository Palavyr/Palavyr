#######################

#  The initial block here defines this as a terraform file
#  When this gets deployed, the first block with the backend "remote"
#  code needs to be uncommented.


#  Below, where it says "For dev time" - you can use this for local tracking if you wish
#  Comment out remote terraform block, and uncomment the local block

##################
# terraform {
#   backend "remote" {
#     organization = "palavyr"
#     token        = "#{TerraformCloudApiToken}"

#     workspaces {
#       name = "#{TerraformWorkspace}"
#     }
#   }

#   required_providers {
#     aws = {
#       source  = "hashicorp/aws"
#       version = "~> 4.0"
#     }
#   }
# }

#######
# Uncomment the code below when working locally if you want to track your
# own infra while testing in development
#######

terraform {
  backend "local" {
  }
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 4.0"
    }
  }
}


provider "aws" {
  alias  = "aws"
  region = "us-east-1"
}


####################################################################

# resource "aws_cloudfront_origin_access_identity" "oai" {
#   comment = "OAI for ${var.hosted_zone_domain_name}"
# }

# module "configuration_app_website" {
#   source                       = "./modules/website"
#   aws_region                   = var.aws_region
#   bucket_prefix                = "${lower(var.environment)}-${var.app_domain_name}-"
#   site_domain_name             = var.app_domain_name
#   environment                  = var.environment
#   hosted_zone_domain_name      = var.hosted_zone_domain_name
#   cloudfront_access_id_path    = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
#   cloudfront_access_id_iam_arn = aws_cloudfront_origin_access_identity.oai.iam_arn
# }

# module "widget_app_website" {
#   source                       = "./modules/website"
#   aws_region                   = var.aws_region
#   bucket_prefix                = "${lower(var.environment)}-${var.widget_domain_name}"
#   site_domain_name             = var.widget_domain_name
#   environment                  = var.environment
#   hosted_zone_domain_name      = var.hosted_zone_domain_name
#   cloudfront_access_id_path    = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
#   cloudfront_access_id_iam_arn = aws_cloudfront_origin_access_identity.oai.iam_arn

# }

# module "server" {
#   source                  = "./modules/serving"
#   region                  = var.aws_region
#   domain_name             = var.server_domain_name
#   vpc_cidr                = "10.10.0.0/16"
#   environment             = var.environment
#   hosted_zone_domain_name = var.hosted_zone_domain_name
# }

module "pdf_server" {
  source = "./modules/lambda_endpoint"

  environment         = var.environment
  lambda_runtime      = "nodejs12.x"
  lambda_handler_name = "lambda.handler"
  function_name       = "palavyr-pdf-server-${lower(var.environment)}"
  aws_iam_role_name   = "serverless_lambda_${var.environment}"
  region              = var.aws_region
  gateway_name        = "api-gateway-pdf-server-${lower(var.environment)}"
  gateway_stage_name  = lower(var.environment)

  # image_uri = "${var.aws_account_id}.dkr.ecr.${var.aws_region}.amazonaws.com/palavyr/palavyr-pdf-server-lambda:latest"

}

module "palavyr_user_data_bucket" {
  source      = "./modules/data_buckets"
  bucket_name = "palavyr-user-data-${lower(var.environment)}"
}
