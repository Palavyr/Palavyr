# resource "aws_lambda_function" "migrator" {
#   function_name = "palavyr-database-migrator-{${lower(var.environment)}}"
#   runtime       = "dotnet6"
#   handler       = "Palavyr.Data.Migrator::Palavyr.Data.Migrator.MigratorLambdaHandler::MigratorHandler"
#   role          = aws_iam_role.migrator_role.arn

#   s3_bucket = module.migrator_deployment_bucket.data_bucket_name
#   s3_key    = "lambda.zip"

#   depends_on = [
#     module.migrator_deployment_bucket
#   ]

#   tags = local.tags
# }

module "migrator_deployment_bucket" {
  source                = "./modules/data_buckets"
  bucket_name           = "palavyr-migrator-deployment-bucket-${lower(var.environment)}"
  protect_from_deletion = false
  key_alias             = "alias/terraform_deployment_bucket_key_${lower(var.environment)}"

  tags = local.tags
}
