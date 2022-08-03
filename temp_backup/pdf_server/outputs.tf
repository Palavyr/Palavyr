# Lambda function name
output "pdf_server_function_name" {
  description = "Name of the Lambda function."

  value = aws_lambda_function.pdf_server.function_name
}

# Invocation URL for the lambda (via the Api Gateway)
output "pdf_server_url" {
  description = "Invocation URL for API Gateway stage."

  value = aws_apigatewayv2_stage.lambda.invoke_url
}