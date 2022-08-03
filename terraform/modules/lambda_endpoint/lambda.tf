resource "aws_lambda_function" "func" {
  function_name = var.function_name

  runtime = var.lambda_runtime      #"nodejs12.x"
  handler = var.lambda_handler_name #"lambda.handler"

  role = aws_iam_role.lambda_exec.arn

  image_uri    = var.image_uri
  package_type = "Image"

}

resource "aws_iam_role" "lambda_exec" {
  name = var.aws_iam_role_name

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Sid    = ""
      Principal = {
        Service = "lambda.amazonaws.com"
      }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_policy" {
  role       = aws_iam_role.lambda_exec.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}
