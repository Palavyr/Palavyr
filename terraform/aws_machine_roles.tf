resource "aws_iam_role" "migrator_role" {
  name = "migrator-role-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Sid    = ""
      Principal = {
        Service = "lambda.amazonaws.com"
      }
    }]
  })
}

resource "aws_iam_role_policy" "migrator_role_policy" {
  name   = "migrator-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  role   = aws_iam_role.migrator_role.id
  policy = aws_iam_policy.machine_full_access_user_policy.policy
}

resource "aws_iam_role_policy_attachment" "lambda_policy" {
  role       = aws_iam_role.migrator_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}
## ------------------------------------------------------------