resource "aws_lambda_function" "migrator" {
  function_name = "palavyr-database-migrator-{${lower(var.environment)}}"
  runtime       = "dotnet6"
  handler       = "Palavyr.Data.Migrator::Palavyr.Data.Migrator.MigratorLambdaHandler::MigratorHandler"

  role = aws_iam_role.migrator_role.arn

  tags = var.tags
}

# resource "aws_iam_role" "lambda_exec" {
#   name = var.aws_iam_role_name

#   assume_role_policy = jsonencode({
#     Version = "2012-10-17"
#     Statement = [{
#       Action = "sts:AssumeRole"
#       Effect = "Allow"
#       Sid    = ""
#       Principal = {
#         Service = "lambda.amazonaws.com"
#       }
#       }
#     ]
#   })
# }

# resource "aws_iam_role_policy_attachment" "lambda_policy" {
#   role       = aws_iam_role.lambda_exec.name
#   policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
# }
