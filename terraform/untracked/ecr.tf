resource "aws_ecr_repository" "frontend_ecr" {
  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = "false"
  }

  image_tag_mutability = "MUTABLE"
  name                 = "palavyr/palavyr-frontend"
}

resource "aws_ecr_repository" "pdf_server_ecr" {
  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = "false"
  }

  image_tag_mutability = "MUTABLE"
  name                 = "palavyr/palavyr-pdf-server"
}

resource "aws_ecr_repository" "pdf_server_ecr_policy" {
  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = "false"
  }

  image_tag_mutability = "MUTABLE"
  name                 = "palavyr/palavyr-pdf-server-lambda"
}

resource "aws_ecr_repository" "server_ecr" {
  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = "false"
  }

  image_tag_mutability = "MUTABLE"
  name                 = "palavyr/palavyr-server"
}

resource "aws_ecr_repository" "widget_ecr" {
  encryption_configuration {
    encryption_type = "AES256"
  }

  image_scanning_configuration {
    scan_on_push = "false"
  }

  image_tag_mutability = "MUTABLE"
  name                 = "palavyr/palavyr-widget"
}

resource "aws_ecr_repository_policy" "frontend_ecr" {
  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "ecr:BatchCheckLayerAvailability",
        "ecr:BatchGetImage",
        "ecr:CompleteLayerUpload",
        "ecr:GetDownloadUrlForLayer",
        "ecr:InitiateLayerUpload",
        "ecr:PutImage",
        "ecr:UploadLayerPart"
      ],
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::306885252482:user/Palavyr_ECR_control"
      },
      "Sid": "Statement1"
    }
  ],
  "Version": "2008-10-17"
}
POLICY

  repository = "palavyr/palavyr-frontend"
}

resource "aws_ecr_repository_policy" "pdf_server_ecr" {
  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "ecr:BatchCheckLayerAvailability",
        "ecr:BatchGetImage",
        "ecr:CompleteLayerUpload",
        "ecr:GetDownloadUrlForLayer",
        "ecr:InitiateLayerUpload",
        "ecr:PutImage",
        "ecr:UploadLayerPart"
      ],
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::306885252482:user/Palavyr_ECR_control"
      },
      "Sid": "Statement1"
    }
  ],
  "Version": "2008-10-17"
}
POLICY

  repository = "palavyr/palavyr-pdf-server"
}

resource "aws_ecr_repository_policy" "pdf_server_ecr_policy" {
  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "ecr:BatchGetImage",
        "ecr:GetDownloadUrlForLayer",
        "ecr:SetRepositoryPolicy",
        "ecr:DeleteRepositoryPolicy",
        "ecr:GetRepositoryPolicy"
      ],
      "Condition": {
        "StringLike": {
          "aws:sourceArn": "arn:aws:lambda:us-east-1:306885252482:function:*"
        }
      },
      "Effect": "Allow",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Sid": "LambdaECRImageRetrievalPolicy"
    }
  ],
  "Version": "2008-10-17"
}
POLICY

  repository = "palavyr/palavyr-pdf-server-lambda"
}

resource "aws_ecr_repository_policy" "server_ecr" {
  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "ecr:BatchCheckLayerAvailability",
        "ecr:BatchGetImage",
        "ecr:CompleteLayerUpload",
        "ecr:GetDownloadUrlForLayer",
        "ecr:InitiateLayerUpload",
        "ecr:PutImage",
        "ecr:UploadLayerPart"
      ],
      "Effect": "Allow",
      "Principal": {
        "AWS": "arn:aws:iam::306885252482:user/Palavyr_ECR_control"
      },
      "Sid": "Statement1"
    }
  ],
  "Version": "2008-10-17"
}
POLICY

  repository = "palavyr/palavyr-server"
}
