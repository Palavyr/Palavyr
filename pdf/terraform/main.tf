# terraform {
#   required_providers {
#     aws = {
#       source  = "hashicorp/aws"
#       version = "~> 4.0.0"
#     }
#     random = {
#       source  = "hashicorp/random"
#       version = "~> 3.1.0"
#     }
#     archive = {
#       source  = "hashicorp/archive"
#       version = "~> 2.2.0"
#     }
#   }

#   required_version = "~> 1.0"
# }

# provider "aws" {
#   region = var.aws_region
# }

# # Create a Lambda function that will be used to generate a PDF.

# resource "aws_lambda_function" "pdf_server" {
#   function_name = "Palavyr-Pdf_Server-${lower(var.environment)}"

#   runtime = "nodejs12.x"
#   handler = "hello.handler"

#   role = aws_iam_role.lambda_exec.arn
# }

# resource "aws_cloudwatch_log_group" "pdf_server" {
#   name = "/aws/lambda/${aws_lambda_function.pdf_server.function_name}"

#   retention_in_days = 10
# }

# resource "aws_iam_role" "lambda_exec" {
#   name = "serverless_lambda"

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
#   policy_arn = "arn:aws:iam::aws:policy/service-role/PalavyrServerless"
# }


# # Create an Api Gateway that will server as the endpoint for the lambda above

# resource "aws_apigatewayv2_api" "lambda" {
#   name          = "serverless_lambda_gw_${lower(var.environment)}"
#   protocol_type = "HTTP"
# }

# resource "aws_apigatewayv2_stage" "lambda" {
#   api_id = aws_apigatewayv2_api.lambda.id

#   name        = "serverless_lambda_stage_${lower(var.environment)}"
#   auto_deploy = true

#   access_log_settings {
#     destination_arn = aws_cloudwatch_log_group.api_gw.arn

#     format = jsonencode({
#       requestId               = "$context.requestId"
#       sourceIp                = "$context.identity.sourceIp"
#       requestTime             = "$context.requestTime"
#       protocol                = "$context.protocol"
#       httpMethod              = "$context.httpMethod"
#       resourcePath            = "$context.resourcePath"
#       routeKey                = "$context.routeKey"
#       status                  = "$context.status"
#       responseLength          = "$context.responseLength"
#       integrationErrorMessage = "$context.integrationErrorMessage"
#       }
#     )
#   }
# }

# resource "aws_apigatewayv2_integration" "pdf_server" {
#   api_id = aws_apigatewayv2_api.lambda.id

#   integration_uri    = aws_lambda_function.pdf_server.invoke_arn
#   integration_type   = "AWS_PROXY"
#   integration_method = "POST"
# }

# resource "aws_apigatewayv2_route" "pdf_server" {
#   api_id = aws_apigatewayv2_api.lambda.id

#   route_key = "GET /pdf"
#   target    = "integrations/${aws_apigatewayv2_integration.pdf_server.id}"
# }

# resource "aws_cloudwatch_log_group" "api_gw" {
#   name = "/aws/api_gw/${aws_apigatewayv2_api.lambda.name}"

#   retention_in_days = 30
# }

# resource "aws_lambda_permission" "api_gw" {
#   statement_id  = "AllowExecutionFromAPIGateway"
#   action        = "lambda:InvokeFunction"
#   function_name = aws_lambda_function.pdf_server.function_name
#   principal     = "apigateway.amazonaws.com"

#   source_arn = "${aws_apigatewayv2_api.lambda.execution_arn}/*/*"
# }
