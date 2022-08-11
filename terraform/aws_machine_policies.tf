# This group will have this policy attached to it
resource "aws_iam_policy" "machine_full_access_user_policy" {
  name        = "machine-fap-${lower(var.environment)}-${lower(random_id.rand.hex)}"
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
  name        = "migrator-policy-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  description = "Database migrator specific access for ${lower(var.environment)}-${lower(random_id.rand.hex)}"

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
