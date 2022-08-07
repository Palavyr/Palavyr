# This group will have this policy attached to it
resource "aws_iam_policy" "machine_full_access_user_policy" {
  name        = "machine_full_access_policy-${lower(var.environment)}"
  description = "General human user access"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        "Sid" : "VisualEditor0",
        "Effect" : "Allow",
        "Action" : [
          "execute-api:*",
          "rds:*",
          "apigateway:*",
          "s3:*",
          "ses:*",
          "cloudwatch:*",
          "logs:*",
          "rds-data:*",
          "lambda:*",
          "ec2:*"
        ],
        "Resource" : "*"
      }
    ]
  })
}
