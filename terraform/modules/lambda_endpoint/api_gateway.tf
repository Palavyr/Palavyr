resource "aws_apigatewayv2_api" "gateway" {
  name          = var.gateway_name
  protocol_type = "HTTP"
}

resource "aws_cloudwatch_log_group" "func" {
  name = "/aws/lambda/${aws_lambda_function.func.function_name}"

  retention_in_days = 30
}


resource "aws_apigatewayv2_stage" "gateway" {
  api_id      = aws_apigatewayv2_api.gateway.id
  name        = var.gateway_stage_name
  auto_deploy = true

  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.func.arn

    format = jsonencode({
      requestId               = "$context.requestId"
      sourceIp                = "$context.identity.sourceIp"
      requestTime             = "$context.requestTime"
      protocol                = "$context.protocol"
      httpMethod              = "$context.httpMethod"
      resourcePath            = "$context.resourcePath"
      routeKey                = "$context.routeKey"
      status                  = "$context.status"
      responseLength          = "$context.responseLength"
      integrationErrorMessage = "$context.integrationErrorMessage"
      }
    )
  }
}


resource "aws_apigatewayv2_integration" "gateway" {
  api_id = aws_apigatewayv2_api.gateway.id

  integration_uri    = aws_lambda_function.func.invoke_arn
  integration_type   = "AWS_PROXY"
  integration_method = "POST"
}

resource "aws_apigatewayv2_route" "route" {
  api_id = aws_apigatewayv2_api.gateway.id

  route_key = "GET /pdf"
  target    = "integrations/${aws_apigatewayv2_integration.gateway.id}"
}

resource "aws_cloudwatch_log_group" "gateway" {
  name = "/aws/api_gw/${aws_apigatewayv2_api.gateway.name}"

  retention_in_days = 30
}

resource "aws_lambda_permission" "gateway" {
  statement_id  = "AllowExecutionFromAPIGateway"
  action        = "lambda:InvokeFunction"
  function_name = aws_lambda_function.func.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${aws_apigatewayv2_api.gateway.execution_arn}/*/*"
}
