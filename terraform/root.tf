terraform {
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

# FOR REMOTE STATE MANAGEMENT
# resource "aws_s3_bucket" "terraform_state" {
#   bucket = "terraform-up-and-running-state"
#   # Enable versioning so we can see the full revision history of our
#   # state files
#   versioning {
#     enabled = true
#   }
#   # Enable server-side encryption by default
#   server_side_encryption_configuration {
#     rule {
#       apply_server_side_encryption_by_default {
#         sse_algorithm = "AES256"
#       }
#     }
#   }
# }



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
  image_uri           = "palavyr/palavyr-pdf-server:latest"

  region         = var.aws_region
  gateway_name       = "api-gateway-pdf-server-${lower(var.environment)}"
  gateway_stage_name = lower(var.environment)

}


# module "palavyr_user_data_buckets" {
#   source = "./modules/data_buckets"
# }

# module "palavyr_policies" {
#   source = "./modules/policies"
# }
