# This group will have this policy attached to it
resource "aws_iam_policy" "machine_full_access_user_policy" {
  name        = "machine-full-access-policy-${lower(var.environment)}"
  description = "General machine user access"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        "Sid" : "VisualEditor0",
        "Effect" : "Allow",
        "Action" : [
          "execute-api:*",
          "apigateway:*",
          "s3:*",
          "ses:*",
          "cloudwatch:*",
          "logs:*",
          "rds:*",
          "rds-data:*",
          "lambda:*",
          "ec2:*"
        ],
        "Resource" : "*"
      }
    ]
  })
}


# This group will have this policy attached to it
resource "aws_iam_policy" "migrator_policy" {
  name        = "migrator_policy-${lower(var.environment)}"
  description = "Database migrator specific access for ${lower(var.environment)}"

  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        "Sid" : "VisualEditor0",
        "Effect" : "Allow",
        "Action" : [
          "cloudwatch:*",
          "logs:*",
          "rds:*",
          "rds-data:*",
          "lambda:*"
        ],
        "Resource" : "*"
      }
    ]
  })
}
