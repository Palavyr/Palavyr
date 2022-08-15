# resource "aws_lambda_function" "migrator" {
#   function_name = "palavyr-db-migrator-${lower(var.environment)}-${lower(random_id.rand.hex)}"
#   runtime       = "dotnet6"
#   handler       = "Palavyr.Data.Migrator::Palavyr.Data.Migrator.MigratorLambdaHandler::MigratorHandler"
#   role          = aws_iam_role.migrator_role.arn

#   environment {
#     variables = {
#       "Palavyr_Environment"      = var.environment
#       "Palavyr_ConnectionString" = module.database.connection_string
#     }
#   }
#   filename = "./dummy_uploads/dummy_lambda_dotnet.zip"


#   tags = local.tags
# }
