resource "aws_acm_certificate" "tfer--2e5f5a9f-002D-92e3-002D-44a5-002D-8856-002D-fcfafbd61e4e_widget-002E-palavyr-002E-com" {
  domain_name = "widget.palavyr.com"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }

  subject_alternative_names = ["widget.palavyr.com", "*.widget.palavyr.com"]

  tags = {
    palavyr = "cert"
  }

  tags_all = {
    palavyr = "cert"
  }

  validation_method = "DNS"
}

resource "aws_acm_certificate" "tfer--54f8b3fb-002D-fa4a-002D-43fa-002D-aee5-002D-d17c75cabca1_app-002E-palavyr-002E-com" {
  domain_name = "app.palavyr.com"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }

  subject_alternative_names = ["*.app.palavyr.com", "app.palavyr.com"]

  tags = {
    palavyr = "cert"
  }

  tags_all = {
    palavyr = "cert"
  }

  validation_method = "DNS"
}

resource "aws_acm_certificate" "tfer--75aa80d1-002D-4cfb-002D-42c8-002D-9bc6-002D-97beebb997da_staging-002E-app-002E-palavyr-002E-com" {
  domain_name = "staging.app.palavyr.com"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }

  subject_alternative_names = ["www.staging.app.palavyr.com", "staging.app.palavyr.com"]

  tags = {
    apppalavyr = "cert"
  }

  tags_all = {
    apppalavyr = "cert"
  }

  validation_method = "DNS"
}

resource "aws_acm_certificate" "tfer--8ec5fa03-002D-cd67-002D-465d-002D-be94-002D-2ae3b064c2c0_server-002E-palavyr-002E-com" {
  domain_name = "server.palavyr.com"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }

  subject_alternative_names = ["*.server.palavyr.com", "server.palavyr.com"]

  tags = {
    ServerCert = "Cert"
  }

  tags_all = {
    ServerCert = "Cert"
  }

  validation_method = "DNS"
}

resource "aws_acm_certificate" "tfer--ec1a02a1-002D-ab61-002D-48f2-002D-8ebd-002D-d7ead697c1ac_palavyr-002E-com" {
  domain_name = "palavyr.com"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }

  subject_alternative_names = ["palavyr.com", "*.palavyr.com"]

  tags = {
    palavyr = "cert"
  }

  tags_all = {
    palavyr = "cert"
  }

  validation_method = "DNS"
}

resource "aws_cloudfront_distribution" "tfer--E2RYUIRP8XMKQG" {
  aliases = ["widget.palavyr.com", "www.widget.palavyr.com"]
  comment = "Distribution to handle SSL for Palavyr widget"

  custom_error_response {
    error_caching_min_ttl = "10"
    error_code            = "403"
    response_code         = "200"
    response_page_path    = "/index.html"
  }

  custom_error_response {
    error_caching_min_ttl = "10"
    error_code            = "404"
    response_code         = "200"
    response_page_path    = "/index.html"
  }

  default_cache_behavior {
    allowed_methods        = ["GET", "HEAD"]
    cache_policy_id        = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"
    cached_methods         = ["HEAD", "GET"]
    compress               = "false"
    default_ttl            = "0"
    max_ttl                = "0"
    min_ttl                = "0"
    smooth_streaming       = "false"
    target_origin_id       = "S3 Palavyr Widget Access"
    viewer_protocol_policy = "redirect-to-https"
  }

  default_root_object = "index.html"
  enabled             = "true"
  http_version        = "http2"
  is_ipv6_enabled     = "true"

  origin {
    connection_attempts = "3"
    connection_timeout  = "10"

    custom_origin_config {
      http_port                = "80"
      https_port               = "443"
      origin_keepalive_timeout = "5"
      origin_protocol_policy   = "http-only"
      origin_read_timeout      = "30"
      origin_ssl_protocols     = ["TLSv1.2", "TLSv1.1", "TLSv1"]
    }

    domain_name = "widget.palavyr.com.s3-website-us-east-1.amazonaws.com"
    origin_id   = "S3 Palavyr Widget Access"
  }

  price_class = "PriceClass_All"

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  retain_on_delete = "false"

  viewer_certificate {
    acm_certificate_arn            = "arn:aws:acm:us-east-1:306885252482:certificate/2e5f5a9f-92e3-44a5-8856-fcfafbd61e4e"
    cloudfront_default_certificate = "false"
    minimum_protocol_version       = "TLSv1.2_2019"
    ssl_support_method             = "sni-only"
  }
}

resource "aws_cloudfront_distribution" "tfer--E31M45VVJZ3I65" {
  aliases = ["app.palavyr.com", "www.app.palavyr.com"]
  comment = "palavyr.com static hosting"

  custom_error_response {
    error_caching_min_ttl = "10"
    error_code            = "403"
    response_code         = "200"
    response_page_path    = "/index.html"
  }

  custom_error_response {
    error_caching_min_ttl = "10"
    error_code            = "404"
    response_code         = "200"
    response_page_path    = "/index.html"
  }

  default_cache_behavior {
    allowed_methods        = ["HEAD", "GET"]
    cache_policy_id        = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"
    cached_methods         = ["GET", "HEAD"]
    compress               = "false"
    default_ttl            = "0"
    max_ttl                = "0"
    min_ttl                = "0"
    smooth_streaming       = "false"
    target_origin_id       = "S3-Palavyr.com"
    viewer_protocol_policy = "redirect-to-https"
  }

  default_root_object = "index.html"
  enabled             = "true"
  http_version        = "http2"
  is_ipv6_enabled     = "true"

  origin {
    connection_attempts = "3"
    connection_timeout  = "10"

    custom_origin_config {
      http_port                = "80"
      https_port               = "443"
      origin_keepalive_timeout = "5"
      origin_protocol_policy   = "http-only"
      origin_read_timeout      = "30"
      origin_ssl_protocols     = ["TLSv1.2", "TLSv1.1", "TLSv1"]
    }

    domain_name = "palavyr.com.s3-website-us-east-1.amazonaws.com"
    origin_id   = "S3-Palavyr.com"
  }

  price_class = "PriceClass_100"

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  retain_on_delete = "false"

  viewer_certificate {
    acm_certificate_arn            = "arn:aws:acm:us-east-1:306885252482:certificate/54f8b3fb-fa4a-43fa-aee5-d17c75cabca1"
    cloudfront_default_certificate = "false"
    minimum_protocol_version       = "TLSv1.2_2019"
    ssl_support_method             = "sni-only"
  }
}

resource "aws_cloudfront_distribution" "tfer--E3CTPE4KWZEPXM" {
  aliases = ["staging.widget.palavyr.com"]
  comment = "staging widget front"

  default_cache_behavior {
    allowed_methods          = ["GET", "OPTIONS", "HEAD"]
    cache_policy_id          = "4135ea2d-6df8-44a3-9df3-4b5a84be39ad"
    cached_methods           = ["HEAD", "GET"]
    compress                 = "false"
    default_ttl              = "0"
    max_ttl                  = "0"
    min_ttl                  = "0"
    origin_request_policy_id = "88a5eaf4-2fd4-4709-b370-b4c650ea3fcf"
    smooth_streaming         = "false"
    target_origin_id         = "Staging widget "
    viewer_protocol_policy   = "redirect-to-https"
  }

  default_root_object = "index.html"
  enabled             = "true"
  http_version        = "http2"
  is_ipv6_enabled     = "true"

  origin {
    connection_attempts = "3"
    connection_timeout  = "10"

    custom_origin_config {
      http_port                = "80"
      https_port               = "443"
      origin_keepalive_timeout = "5"
      origin_protocol_policy   = "http-only"
      origin_read_timeout      = "30"
      origin_ssl_protocols     = ["TLSv1", "TLSv1.2", "TLSv1.1"]
    }

    domain_name = "staging.widget.palavyr.com.s3-website-us-east-1.amazonaws.com"
    origin_id   = "Staging widget "
  }

  price_class = "PriceClass_All"

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  retain_on_delete = "false"

  viewer_certificate {
    acm_certificate_arn            = "arn:aws:acm:us-east-1:306885252482:certificate/2e5f5a9f-92e3-44a5-8856-fcfafbd61e4e"
    cloudfront_default_certificate = "false"
    minimum_protocol_version       = "TLSv1.2_2019"
    ssl_support_method             = "sni-only"
  }
}

resource "aws_cloudfront_distribution" "tfer--EGLTO43ZN66KW" {
  aliases = ["www.staging.app.palavyr.com", "staging.app.palavyr.com"]
  comment = "staging app palavyr . com"

  default_cache_behavior {
    allowed_methods        = ["GET", "HEAD"]
    cache_policy_id        = "658327ea-f89d-4fab-a63d-7e88639e58f6"
    cached_methods         = ["HEAD", "GET"]
    compress               = "true"
    default_ttl            = "0"
    max_ttl                = "0"
    min_ttl                = "0"
    smooth_streaming       = "false"
    target_origin_id       = "www.staging.palavyr.com.s3.us-east-1.amazonaws.com"
    viewer_protocol_policy = "allow-all"
  }

  default_root_object = "index.html"
  enabled             = "true"
  http_version        = "http2"
  is_ipv6_enabled     = "true"

  origin {
    connection_attempts = "3"
    connection_timeout  = "10"
    domain_name         = "staging.palavyr.com.s3.us-east-1.amazonaws.com"
    origin_id           = "www.staging.palavyr.com.s3.us-east-1.amazonaws.com"
  }

  price_class = "PriceClass_All"

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  retain_on_delete = "false"

  viewer_certificate {
    acm_certificate_arn            = "arn:aws:acm:us-east-1:306885252482:certificate/75aa80d1-4cfb-42c8-9bc6-97beebb997da"
    cloudfront_default_certificate = "false"
    minimum_protocol_version       = "TLSv1.2_2021"
    ssl_support_method             = "sni-only"
  }
}

resource "aws_cloudtrail" "tfer--SES-002D-palavyr" {
  enable_log_file_validation    = "false"
  enable_logging                = "true"
  include_global_service_events = "true"
  is_multi_region_trail         = "true"
  is_organization_trail         = "false"
  name                          = "SES-palavyr"
  s3_bucket_name                = "aws-cloudtrail-logs-306885252482-eeb5c23b"
}

resource "aws_iam_group" "tfer--PalavyrServices" {
  name = "PalavyrServices"
  path = "/"
}

resource "aws_iam_group" "tfer--Work" {
  name = "Work"
  path = "/"
}

resource "aws_iam_group_policy_attachment" "tfer--PalavyrServices_AmazonS3FullAccess" {
  group      = "PalavyrServices"
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3FullAccess"
}

resource "aws_iam_group_policy_attachment" "tfer--PalavyrServices_AmazonSESFullAccess" {
  group      = "PalavyrServices"
  policy_arn = "arn:aws:iam::aws:policy/AmazonSESFullAccess"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_Billing" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/Billing"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_DataScientist" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/DataScientist"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_DatabaseAdministrator" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/DatabaseAdministrator"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_NetworkAdministrator" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/NetworkAdministrator"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_PowerUserAccess" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/PowerUserAccess"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_SecurityAudit" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/SecurityAudit"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_SupportUser" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/SupportUser"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_SystemAdministrator" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/SystemAdministrator"
}

resource "aws_iam_group_policy_attachment" "tfer--Work_ViewOnlyAccess" {
  group      = "Work"
  policy_arn = "arn:aws:iam::aws:policy/job-function/ViewOnlyAccess"
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-0b00a921-002D-6c7b-002D-422c-002D-ae4b-002D-214e1a9d625e" {
  name = "AWSLambdaBasicExecutionRole-0b00a921-6c7b-422c-ae4b-214e1a9d625e"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:eu-west-2:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:eu-west-2:306885252482:log-group:/aws/lambda/JadeAPILambda:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-0d2e2ad8-002D-8e73-002D-4315-002D-aca1-002D-3ccbfeaf39c5" {
  name = "AWSLambdaBasicExecutionRole-0d2e2ad8-8e73-4315-aca1-3ccbfeaf39c5"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-east-2:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-east-2:306885252482:log-group:/aws/lambda/GetStartedLambdaProxyIntegration:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-7b689fce-002D-d9ff-002D-4995-002D-8a32-002D-7aae7315d4ef" {
  name = "AWSLambdaBasicExecutionRole-7b689fce-d9ff-4995-8a32-7aae7315d4ef"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-east-1:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-east-1:306885252482:log-group:/aws/lambda/PalavyrServerlessProduction:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-8132b576-002D-7495-002D-429a-002D-8cfc-002D-6bd89f83ce10" {
  name = "AWSLambdaBasicExecutionRole-8132b576-7495-429a-8cfc-6bd89f83ce10"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:s3:::*/*",
        "arn:aws:dynamodb:::*/*"
      ],
      "Sid": "VisualEditor0"
    },
    {
      "Action": [
        "s3:*",
        "apigateway:*",
        "dynamodb:*",
        "lex:*"
      ],
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor1"
    },
    {
      "Action": "ses:*",
      "Effect": "Allow",
      "Resource": [
        "arn:aws:s3:::*/*",
        "arn:aws:dynamodb:::*/*"
      ],
      "Sid": "VisualEditor2"
    },
    {
      "Action": "ses:*",
      "Effect": "Allow",
      "Resource": [
        "arn:aws:s3:::*/*",
        "arn:aws:dynamodb:::*/*"
      ],
      "Sid": "VisualEditor3"
    },
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": [
        "arn:aws:s3:::*/*",
        "arn:aws:dynamodb:::*/*"
      ],
      "Sid": "VisualEditor4"
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-9095598c-002D-f416-002D-4da4-002D-b440-002D-35d2d8dab7fe" {
  name = "AWSLambdaBasicExecutionRole-9095598c-f416-4da4-b440-35d2d8dab7fe"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-east-1:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-east-1:306885252482:log-group:/aws/lambda/StagingServerLambda:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-99d6b2b1-002D-6fd3-002D-4a48-002D-b6eb-002D-944e26a195c8" {
  name = "AWSLambdaBasicExecutionRole-99d6b2b1-6fd3-4a48-b6eb-944e26a195c8"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-west-2:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-west-2:306885252482:log-group:/aws/lambda/solegal_response_validation:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-cbaf6ace-002D-5d98-002D-4a76-002D-a68c-002D-4d138ea54dc8" {
  name = "AWSLambdaBasicExecutionRole-cbaf6ace-5d98-4a76-a68c-4d138ea54dc8"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-west-2:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-west-2:306885252482:log-group:/aws/lambda/JadeAppUserAuthentication:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-cc66e372-002D-3c83-002D-4a3d-002D-ad67-002D-4be96464cded" {
  name = "AWSLambdaBasicExecutionRole-cc66e372-3c83-4a3d-ad67-4be96464cded"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "logs:CreateLogGroup",
      "Effect": "Allow",
      "Resource": "arn:aws:logs:us-east-1:306885252482:*"
    },
    {
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:us-east-1:306885252482:log-group:/aws/lambda/TestFunctionDOTNET:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaBasicExecutionRole-002D-e9da4874-002D-f40f-002D-45e2-002D-be32-002D-89904f85dafb" {
  name = "AWSLambdaBasicExecutionRole-e9da4874-f40f-45e2-be32-89904f85dafb"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "s3:ListAllMyBuckets",
        "s3:GetBucketLocation"
      ],
      "Effect": "Allow",
      "Resource": "*"
    },
    {
      "Action": "s3:*",
      "Effect": "Allow",
      "Resource": [
        "arn:aws:s3:::jade-app-staging/*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaEdgeExecutionRole-002D-2efd2483-002D-8c17-002D-4b47-002D-81b5-002D-c2cbe15d3584" {
  name = "AWSLambdaEdgeExecutionRole-2efd2483-8c17-4b47-81b5-c2cbe15d3584"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "logs:CreateLogGroup",
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Effect": "Allow",
      "Resource": [
        "arn:aws:logs:*:*:*"
      ]
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaSESExecutionRole-002D-780b1287-002D-da95-002D-416e-002D-9475-002D-38d9ff947cdf" {
  name = "AWSLambdaSESExecutionRole-780b1287-da95-416e-9475-38d9ff947cdf"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "ses:SendBounce"
      ],
      "Effect": "Allow",
      "Resource": "*"
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--AWSLambdaTracerAccessExecutionRole-002D-319dcf7f-002D-9a4c-002D-4dff-002D-b051-002D-0d76917eda3a" {
  name = "AWSLambdaTracerAccessExecutionRole-319dcf7f-9a4c-4dff-b051-0d76917eda3a"
  path = "/service-role/"

  policy = <<POLICY
{
  "Statement": {
    "Action": [
      "xray:PutTraceSegments",
      "xray:PutTelemetryRecords"
    ],
    "Effect": "Allow",
    "Resource": [
      "*"
    ]
  },
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--JadeAppMasterPolicy" {
  description = "The master policy used by TheJadeBot"
  name        = "JadeAppMasterPolicy"
  path        = "/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "apigateway:*",
        "s3:*",
        "logs:*",
        "dynamodb:*",
        "lex:*"
      ],
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor0"
    },
    {
      "Action": "ses:*",
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor1"
    },
    {
      "Action": "ses:*",
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor2"
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--Palavyr-002D-AccessControl" {
  description = "The access policies for each of the services used by Palavyr."
  name        = "Palavyr-AccessControl"
  path        = "/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
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
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor0"
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_policy" "tfer--PalavyrAwsPermissionSet" {
  description = "Full permission set required for the Palavyr Web App to function serverless."
  name        = "PalavyrAwsPermissionSet"
  path        = "/"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": [
        "rds:*",
        "s3:*",
        "apigateway:*",
        "ses:*",
        "rds-data:*",
        "lambda:*"
      ],
      "Effect": "Allow",
      "Resource": "*",
      "Sid": "VisualEditor0"
    }
  ],
  "Version": "2012-10-17"
}
POLICY
}

resource "aws_iam_role" "tfer--AWSServiceRoleForCloudFrontLogger" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "logger.cloudfront.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSCloudFrontLogger"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForCloudFrontLogger"
  path                 = "/aws-service-role/logger.cloudfront.amazonaws.com/"
}

resource "aws_iam_role" "tfer--AWSServiceRoleForElasticLoadBalancing" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "elasticloadbalancing.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Allows ELB to call AWS services on your behalf."
  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSElasticLoadBalancingServiceRolePolicy"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForElasticLoadBalancing"
  path                 = "/aws-service-role/elasticloadbalancing.amazonaws.com/"
}

resource "aws_iam_role" "tfer--AWSServiceRoleForLambdaReplicator" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "replicator.lambda.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSLambdaReplicator"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForLambdaReplicator"
  path                 = "/aws-service-role/replicator.lambda.amazonaws.com/"
}


resource "aws_iam_role" "tfer--AWSServiceRoleForRDS" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "rds.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Allows Amazon RDS to manage AWS resources on your behalf"
  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AmazonRDSServiceRolePolicy"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForRDS"
  path                 = "/aws-service-role/rds.amazonaws.com/"
}

resource "aws_iam_role" "tfer--AWSServiceRoleForSupport" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "support.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Enables resource access for AWS to provide billing, administrative and support services"
  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSSupportServiceRolePolicy"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForSupport"
  path                 = "/aws-service-role/support.amazonaws.com/"
}

resource "aws_iam_role" "tfer--AWSServiceRoleForTrustedAdvisor" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "trustedadvisor.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Access for the AWS Trusted Advisor Service to help reduce cost, increase performance, and improve security of your AWS environment."
  managed_policy_arns  = ["arn:aws:iam::aws:policy/aws-service-role/AWSTrustedAdvisorServiceRolePolicy"]
  max_session_duration = "3600"
  name                 = "AWSServiceRoleForTrustedAdvisor"
  path                 = "/aws-service-role/trustedadvisor.amazonaws.com/"
}

resource "aws_iam_role" "tfer--Iraj-002D-Tutoring" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "amplify.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  description          = "Allows Amplify Backend Deployment to access AWS resources on your behalf."
  managed_policy_arns  = ["arn:aws:iam::aws:policy/AdministratorAccess-Amplify"]
  max_session_duration = "3600"
  name                 = "Iraj-Tutoring"
  path                 = "/"
}

resource "aws_iam_role" "tfer--PalavyrServerless" {
  assume_role_policy = <<POLICY
{
  "Statement": [
    {
      "Action": "sts:AssumeRole",
      "Effect": "Allow",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      }
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  managed_policy_arns  = ["arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole", "arn:aws:iam::306885252482:policy/Palavyr-AccessControl"]
  max_session_duration = "3600"
  name                 = "PalavyrServerless"
  path                 = "/service-role/"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForCloudFrontLogger_AWSCloudFrontLogger" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSCloudFrontLogger"
  role       = "AWSServiceRoleForCloudFrontLogger"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForElasticLoadBalancing_AWSElasticLoadBalancingServiceRolePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSElasticLoadBalancingServiceRolePolicy"
  role       = "AWSServiceRoleForElasticLoadBalancing"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForLambdaReplicator_AWSLambdaReplicator" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSLambdaReplicator"
  role       = "AWSServiceRoleForLambdaReplicator"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForRDS_AmazonRDSServiceRolePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AmazonRDSServiceRolePolicy"
  role       = "AWSServiceRoleForRDS"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForSupport_AWSSupportServiceRolePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSSupportServiceRolePolicy"
  role       = "AWSServiceRoleForSupport"
}

resource "aws_iam_role_policy_attachment" "tfer--AWSServiceRoleForTrustedAdvisor_AWSTrustedAdvisorServiceRolePolicy" {
  policy_arn = "arn:aws:iam::aws:policy/aws-service-role/AWSTrustedAdvisorServiceRolePolicy"
  role       = "AWSServiceRoleForTrustedAdvisor"
}

resource "aws_iam_role_policy_attachment" "tfer--Iraj-002D-Tutoring_AdministratorAccess-002D-Amplify" {
  policy_arn = "arn:aws:iam::aws:policy/AdministratorAccess-Amplify"
  role       = "Iraj-Tutoring"
}

resource "aws_iam_role_policy_attachment" "tfer--PalavyrServerless_AWSLambdaVPCAccessExecutionRole" {
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole"
  role       = "PalavyrServerless"
}

resource "aws_iam_role_policy_attachment" "tfer--PalavyrServerless_Palavyr-002D-AccessControl" {
  policy_arn = "arn:aws:iam::306885252482:policy/Palavyr-AccessControl"
  role       = "PalavyrServerless"
}

resource "aws_iam_user" "tfer--AIDAJG2BANO4WNIEDFJ3S" {
  force_destroy = "false"
  name          = "Paul_Gradie"
  path          = "/"
}

resource "aws_iam_user_group_membership" "tfer--Palavyr-002F-PalavyrServices" {
  groups = ["PalavyrServices"]
  user   = "Palavyr"
}

resource "aws_iam_user_group_membership" "tfer--Paul_Gradie-002F-Work" {
  groups = ["Work"]
  user   = "Paul_Gradie"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AWSCloudFormationFullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AWSCloudFormationFullAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AWSLambda_FullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AWSLambda_FullAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AdministratorAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AdministratorAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AmazonRDSFullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonRDSFullAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AmazonS3FullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3FullAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_AmazonSESFullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/AmazonSESFullAccess"
  user       = "Palavyr"
}

resource "aws_iam_user_policy_attachment" "tfer--Palavyr_ResourceGroupsandTagEditorFullAccess" {
  policy_arn = "arn:aws:iam::aws:policy/ResourceGroupsandTagEditorFullAccess"
  user       = "Palavyr"
}

resource "aws_instance" "tfer--i-002D-080f76ecdd5beceb2_New-0020-Production-0020-Palavyr" {
  ami                         = "ami-0b17e49efb8d755c3"
  associate_public_ip_address = "true"
  availability_zone           = "us-east-1a"

  capacity_reservation_specification {
    capacity_reservation_preference = "open"
  }

  cpu_core_count       = "1"
  cpu_threads_per_core = "1"

  credit_specification {
    cpu_credits = "standard"
  }

  disable_api_stop        = "false"
  disable_api_termination = "false"
  ebs_optimized           = "false"

  enclave_options {
    enabled = "false"
  }

  get_password_data                    = "false"
  hibernation                          = "false"
  instance_initiated_shutdown_behavior = "stop"
  instance_type                        = "t2.small"
  ipv6_address_count                   = "0"
  key_name                             = "PalavyrPem2021"

  maintenance_options {
    auto_recovery = "default"
  }

  metadata_options {
    http_endpoint               = "enabled"
    http_put_response_hop_limit = "1"
    http_tokens                 = "optional"
    instance_metadata_tags      = "disabled"
  }

  monitoring = "false"

  private_dns_name_options {
    enable_resource_name_dns_a_record    = "false"
    enable_resource_name_dns_aaaa_record = "false"
    hostname_type                        = "ip-name"
  }

  private_ip = "10.0.97.58"

  root_block_device {
    delete_on_termination = "true"
    encrypted             = "false"

    tags = {
      Palavyr = "EC2"
    }

    volume_size = "30"
    volume_type = "gp2"
  }

  source_dest_check = "true"
  subnet_id         = data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0ccabdc7222055adf_id

  tags = {
    Name    = "New Production Palavyr"
    Palavyr = "EC2"
  }

  tags_all = {
    Name    = "New Production Palavyr"
    Palavyr = "EC2"
  }

  tenancy                = "default"
  vpc_security_group_ids = ["${data.terraform_remote_state.local.outputs.aws_security_group_tfer--Palavyr-002D-Security_sg-002D-0b658d1cd68796271_id}"]
}

resource "aws_internet_gateway" "tfer--igw-002D-01e0fa378e8d55a55" {
  tags = {
    Name = "palavyr-internet-gateway"
  }

  tags_all = {
    Name = "palavyr-internet-gateway"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_lb" "tfer--production-002D-palavyr-002D-loadbalancer" {
  desync_mitigation_mode     = "defensive"
  drop_invalid_header_fields = "false"
  enable_deletion_protection = "false"
  enable_http2               = "true"
  enable_waf_fail_open       = "false"
  idle_timeout               = "60"
  internal                   = "false"
  ip_address_type            = "ipv4"
  load_balancer_type         = "application"
  name                       = "production-palavyr-loadbalancer"
  security_groups            = ["${data.terraform_remote_state.local.outputs.aws_security_group_tfer--Palavyr-002D-Security_sg-002D-0b658d1cd68796271_id}"]

  subnet_mapping {
    subnet_id = "subnet-0ccabdc7222055adf"
  }

  subnet_mapping {
    subnet_id = "subnet-0704cbef441993b25"
  }

  subnets = ["${data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0704cbef441993b25_id}", "${data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0ccabdc7222055adf_id}"]

  tags = {
    Palavyr = "ProductionLoadBalancer"
  }

  tags_all = {
    Palavyr = "ProductionLoadBalancer"
  }
}

resource "aws_lb_listener" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-listener-002F-app-002F-production-002D-palavyr-002D-loadbalancer-002F-efce1837c433d91c-002F-a4a1d0728da3595f" {
  certificate_arn = "arn:aws:acm:us-east-1:306885252482:certificate/8ec5fa03-cd67-465d-be94-2ae3b064c2c0"

  default_action {
    target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
    type             = "forward"
  }

  load_balancer_arn = data.terraform_remote_state.local.outputs.aws_lb_tfer--production-002D-palavyr-002D-loadbalancer_id
  port              = "443"
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-2016-08"
}

resource "aws_lb_listener" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-listener-002F-app-002F-production-002D-palavyr-002D-loadbalancer-002F-efce1837c433d91c-002F-e056056495b34808" {
  default_action {
    target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
    type             = "forward"
  }

  load_balancer_arn = data.terraform_remote_state.local.outputs.aws_lb_tfer--production-002D-palavyr-002D-loadbalancer_id
  port              = "80"
  protocol          = "HTTP"
}

resource "aws_lb_target_group" "tfer--Palavyr-002D-Production-002D-Https-002D-to-002D-Http" {
  deregistration_delay = "300"

  health_check {
    enabled             = "true"
    healthy_threshold   = "5"
    interval            = "30"
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = "5"
    unhealthy_threshold = "2"
  }

  load_balancing_algorithm_type = "round_robin"
  name                          = "Palavyr-Production-Https-to-Http"
  port                          = "80"
  protocol                      = "HTTP"
  protocol_version              = "HTTP1"
  slow_start                    = "0"

  stickiness {
    cookie_duration = "86400"
    enabled         = "false"
    type            = "lb_cookie"
  }

  target_type = "instance"
  vpc_id      = "vpc-7d14a107"
}

resource "aws_lb_target_group" "tfer--Palavyr-002D-Staging-002D-Https-002D-to-002D-Http" {
  deregistration_delay = "300"

  health_check {
    enabled             = "true"
    healthy_threshold   = "5"
    interval            = "30"
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = "5"
    unhealthy_threshold = "2"
  }

  load_balancing_algorithm_type = "round_robin"
  name                          = "Palavyr-Staging-Https-to-Http"
  port                          = "80"
  protocol                      = "HTTP"
  protocol_version              = "HTTP1"
  slow_start                    = "0"

  stickiness {
    cookie_duration = "86400"
    enabled         = "false"
    type            = "lb_cookie"
  }

  target_type = "instance"
  vpc_id      = "vpc-7d14a107"
}

resource "aws_lb_target_group" "tfer--production-002D-palavyr-002D-targets" {
  deregistration_delay = "300"

  health_check {
    enabled             = "true"
    healthy_threshold   = "5"
    interval            = "30"
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = "5"
    unhealthy_threshold = "2"
  }

  load_balancing_algorithm_type = "round_robin"
  name                          = "production-palavyr-targets"
  port                          = "80"
  protocol                      = "HTTP"
  protocol_version              = "HTTP1"
  slow_start                    = "0"

  stickiness {
    cookie_duration = "86400"
    enabled         = "false"
    type            = "lb_cookie"
  }

  tags = {
    Palavyr = "rds production"
  }

  tags_all = {
    Palavyr = "rds production"
  }

  target_type = "instance"
  vpc_id      = "vpc-0658b6edff9ad6795"
}

resource "aws_lb_target_group" "tfer--staging-002D-palavyr-002D-targets" {
  deregistration_delay = "300"

  health_check {
    enabled             = "true"
    healthy_threshold   = "5"
    interval            = "30"
    matcher             = "200"
    path                = "/healthcheck"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = "5"
    unhealthy_threshold = "2"
  }

  load_balancing_algorithm_type = "round_robin"
  name                          = "staging-palavyr-targets"
  port                          = "80"
  protocol                      = "HTTP"
  protocol_version              = "HTTP1"
  slow_start                    = "0"

  stickiness {
    cookie_duration = "86400"
    enabled         = "false"
    type            = "lb_cookie"
  }

  target_type = "instance"
  vpc_id      = "vpc-0658b6edff9ad6795"
}

resource "aws_lb_target_group_attachment" "tfer--arn-003A-aws-003A-elasticloadbalancing-003A-us-002D-east-002D-1-003A-306885252482-003A-targetgroup-002F-production-002D-palavyr-002D-targets-002F-0883c2b756bf8cd3-002D-20220730012450999700000001" {
  target_group_arn = "arn:aws:elasticloadbalancing:us-east-1:306885252482:targetgroup/production-palavyr-targets/0883c2b756bf8cd3"
  target_id        = "i-080f76ecdd5beceb2"
}

resource "aws_main_route_table_association" "tfer--vpc-002D-0658b6edff9ad6795" {
  route_table_id = data.terraform_remote_state.local.outputs.aws_route_table_tfer--rtb-002D-04e0b491f4190f5f4_id
  vpc_id         = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_network_acl" "tfer--acl-002D-0ec9d3873a453b75e" {
  egress {
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = "0"
    icmp_code  = "0"
    icmp_type  = "0"
    protocol   = "-1"
    rule_no    = "100"
    to_port    = "0"
  }

  ingress {
    action     = "allow"
    cidr_block = "0.0.0.0/0"
    from_port  = "0"
    icmp_code  = "0"
    icmp_type  = "0"
    protocol   = "-1"
    rule_no    = "100"
    to_port    = "0"
  }

  subnet_ids = ["${data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0ccabdc7222055adf_id}", "${data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-03e90beae4cbc97e3_id}", "${data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0704cbef441993b25_id}"]
  vpc_id     = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_network_interface" "tfer--eni-002D-0102422eb42393bf3" {
  description             = "ELB app/production-palavyr-loadbalancer/efce1837c433d91c"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.0.76.233"
  private_ip_list         = ["10.0.76.233"]
  private_ip_list_enabled = "true"
  security_groups         = ["sg-0b658d1cd68796271"]
  source_dest_check       = "true"
  subnet_id               = "subnet-0ccabdc7222055adf"
}

resource "aws_network_interface" "tfer--eni-002D-02dd72ddd3694a443" {
  attachment {
    device_index = "0"
    instance     = "i-080f76ecdd5beceb2"
  }

  description             = "Primary network interface"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.0.97.58"
  private_ip_list         = ["10.0.97.58"]
  private_ip_list_enabled = "true"
  security_groups         = ["sg-0b658d1cd68796271"]
  source_dest_check       = "true"
  subnet_id               = "subnet-0ccabdc7222055adf"

  tags = {
    Palavyr = "EC2"
  }

  tags_all = {
    Palavyr = "EC2"
  }
}

resource "aws_network_interface" "tfer--eni-002D-07e318bdc98fe7f92" {
  description             = "ELB app/production-palavyr-loadbalancer/efce1837c433d91c"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.1.90.140"
  private_ip_list         = ["10.1.90.140"]
  private_ip_list_enabled = "true"
  security_groups         = ["sg-0b658d1cd68796271"]
  source_dest_check       = "true"
  subnet_id               = "subnet-0704cbef441993b25"
}

resource "aws_network_interface" "tfer--eni-002D-08f825c0eb0c2ff0e" {
  description             = "RDSNetworkInterface"
  ipv4_prefix_count       = "0"
  ipv6_address_count      = "0"
  ipv6_prefix_count       = "0"
  private_ip              = "10.1.27.93"
  private_ip_list         = ["10.1.27.93"]
  private_ip_list_enabled = "true"
  security_groups         = ["sg-0b658d1cd68796271"]
  source_dest_check       = "true"
  subnet_id               = "subnet-0704cbef441993b25"
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__2481dde6dcb67a702120ae4352a5215d-002E-app-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_2481dde6dcb67a702120ae4352a5215d.app.palavyr.com"
  records = ["_74ba60a86741d78adc91ffd971a8ef5a.ylqxxscwpq.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__2c90992853fca21ee536026a9a518995-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_2c90992853fca21ee536026a9a518995.palavyr.com"
  records = ["_e3133e5ae6856df5c59618ed11ea05ce.ylqxxscwpq.acm-validations.aws."]
  ttl     = "500"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__2c90992853fca21ee536026a9a518995-002E-palavyr-002E-com-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_2c90992853fca21ee536026a9a518995.palavyr.com.palavyr.com"
  records = ["_e3133e5ae6856df5c59618ed11ea05ce.ylqxxscwpq.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__2e0470f870d6941d634274bd1e7dd086-002E-server-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_2e0470f870d6941d634274bd1e7dd086.server.palavyr.com"
  records = ["14AE5F3438F92AEA9D090829698F3DAD.5462A622E76EDE28FE630D08D0E05725.aa6a75997ff03c7.comodoca.com"]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__5b2cf07b17c29d0ff2ffe7a0377a80f1-002E-www-002E-staging-002E-app-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_5b2cf07b17c29d0ff2ffe7a0377a80f1.www.staging.app.palavyr.com"
  records = ["_d770a4d4506a54e8fe6ccd03c20e52b4.ylqxxscwpq.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__74b6a34e346830857515171cb07f8672-002E-staging-002E-app-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_74b6a34e346830857515171cb07f8672.staging.app.palavyr.com"
  records = ["_b17a814fe2a46cbefd732995d550e27a.ylqxxscwpq.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__94ee71fa70fef497de4ba7d0b9bac392-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_94ee71fa70fef497de4ba7d0b9bac392.palavyr.com"
  records = ["_e672ba393738d6f10b0f35f242a2151e.zdxcnfdgtt.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__c017c56585630b2ba727226472a839b6-002E-widget-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_c017c56585630b2ba727226472a839b6.widget.palavyr.com"
  records = ["_89c066d3949f0d2917ad25c1c5fb2882.zdxcnfdgtt.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU__d471c080522376fc27c8637e66bb6f56-002E-server-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "_d471c080522376fc27c8637e66bb6f56.server.palavyr.com"
  records = ["_53b14a5b617d7faea7b05ef0fffa0124.zdxcnfdgtt.acm-validations.aws."]
  ttl     = "300"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_app-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "d3d02ewghyvfom.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "app.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "d25ob2k1am9duh.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com-002E-_NS_" {
  name    = "palavyr.com"
  records = ["ns-1578.awsdns-05.co.uk.", "ns-803.awsdns-36.net.", "ns-9.awsdns-01.com.", "ns-1321.awsdns-37.org."]
  ttl     = "172800"
  type    = "NS"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com-002E-_SOA_" {
  name    = "palavyr.com"
  records = ["ns-9.awsdns-01.com. awsdns-hostmaster.amazon.com. 1 7200 900 1209600 86400"]
  ttl     = "900"
  type    = "SOA"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com-002E-_TXT_" {
  name    = "palavyr.com"
  records = ["google-site-verification=IckcjSMeYih40iE1g1GDtsRnALsU5SYI0G_Cksztxwk"]
  ttl     = "300"
  type    = "TXT"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_server-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "dualstack.production-palavyr-loadbalancer-844715446.us-east-1.elb.amazonaws.com"
    zone_id                = "Z35SXDOTRQ7X7K"
  }

  name    = "server.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_staging-002E-app-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "d9uue0l9suq2f.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "staging.app.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_staging-002E-server-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "true"
    name                   = "dualstack.staging-palavyr-loadbalancer-645025361.us-east-1.elb.amazonaws.com"
    zone_id                = "Z35SXDOTRQ7X7K"
  }

  name    = "staging.server.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_staging-002E-widget-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "dh2n7jmr2lrle.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "staging.widget.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_widget-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "drtnvpgvsoe97.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "widget.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-app-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "d3d02ewghyvfom.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "www.app.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-palavyr-002E-com-002E-_CNAME_" {
  name    = "www.palavyr.com"
  records = ["d25ob2k1am9duh.cloudfront.net"]
  ttl     = "500"
  type    = "CNAME"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-server-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "dualstack.production-palavyr-loadbalancer-844715446.us-east-1.elb.amazonaws.com"
    zone_id                = "Z35SXDOTRQ7X7K"
  }

  name    = "www.server.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-staging-002E-app-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "d9uue0l9suq2f.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "www.staging.app.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-staging-002E-server-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "true"
    name                   = "dualstack.staging-palavyr-loadbalancer-645025361.us-east-1.elb.amazonaws.com"
    zone_id                = "Z35SXDOTRQ7X7K"
  }

  name    = "www.staging.server.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_record" "tfer--Z0392348306WBDPNO6FWU_www-002E-widget-002E-palavyr-002E-com-002E-_A_" {
  alias {
    evaluate_target_health = "false"
    name                   = "drtnvpgvsoe97.cloudfront.net"
    zone_id                = "Z2FDTNDATAQYW2"
  }

  name    = "www.widget.palavyr.com"
  type    = "A"
  zone_id = aws_route53_zone.tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com.zone_id
}

resource "aws_route53_zone" "tfer--Z0392348306WBDPNO6FWU_palavyr-002E-com" {
  comment       = "HostedZone created by Route53 Registrar"
  force_destroy = "false"
  name          = "palavyr.com"
}

resource "aws_route_table" "tfer--rtb-002D-04e0b491f4190f5f4" {
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = "igw-01e0fa378e8d55a55"
  }

  tags = {
    Name = "Palavyr Subnet"
  }

  tags_all = {
    Name = "Palavyr Subnet"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_route_table" "tfer--rtb-002D-08e25b85cadd3a877" {
  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = "igw-01e0fa378e8d55a55"
  }

  tags = {
    Name = "Public Subnet"
  }

  tags_all = {
    Name = "Public Subnet"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_route_table_association" "tfer--subnet-002D-03e90beae4cbc97e3" {
  route_table_id = data.terraform_remote_state.local.outputs.aws_route_table_tfer--rtb-002D-04e0b491f4190f5f4_id
  subnet_id      = data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-03e90beae4cbc97e3_id
}

resource "aws_route_table_association" "tfer--subnet-002D-0704cbef441993b25" {
  route_table_id = data.terraform_remote_state.local.outputs.aws_route_table_tfer--rtb-002D-04e0b491f4190f5f4_id
  subnet_id      = data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0704cbef441993b25_id
}

resource "aws_route_table_association" "tfer--subnet-002D-0ccabdc7222055adf" {
  route_table_id = data.terraform_remote_state.local.outputs.aws_route_table_tfer--rtb-002D-04e0b491f4190f5f4_id
  subnet_id      = data.terraform_remote_state.local.outputs.aws_subnet_tfer--subnet-002D-0ccabdc7222055adf_id
}

resource "aws_s3_bucket" "tfer--aws-002D-cloudtrail-002D-logs-002D-306885252482-002D-eeb5c23b" {
  arn           = "arn:aws:s3:::aws-cloudtrail-logs-306885252482-eeb5c23b"
  bucket        = "aws-cloudtrail-logs-306885252482-eeb5c23b"
  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetBucketAcl",
      "Effect": "Allow",
      "Principal": {
        "Service": "cloudtrail.amazonaws.com"
      },
      "Resource": "arn:aws:s3:::aws-cloudtrail-logs-306885252482-eeb5c23b",
      "Sid": "AWSCloudTrailAclCheck20150319"
    },
    {
      "Action": "s3:PutObject",
      "Condition": {
        "StringEquals": {
          "AWS:SourceArn": "arn:aws:cloudtrail:us-east-1:306885252482:trail/SES-palavyr",
          "s3:x-amz-acl": "bucket-owner-full-control"
        }
      },
      "Effect": "Allow",
      "Principal": {
        "Service": "cloudtrail.amazonaws.com"
      },
      "Resource": "arn:aws:s3:::aws-cloudtrail-logs-306885252482-eeb5c23b/AWSLogs/306885252482/*",
      "Sid": "AWSCloudTrailWrite20150319"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }
}

resource "aws_s3_bucket" "tfer--palavyr-002E-com" {
  arn    = "arn:aws:s3:::palavyr.com"
  bucket = "palavyr.com"

  cors_rule {
    allowed_headers = ["*"]
    allowed_methods = ["GET", "POST", "PUT"]
    allowed_origins = ["*"]
    max_age_seconds = "25000"
  }

  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Resource": "arn:aws:s3:::palavyr.com/*",
      "Sid": "PublicReadGetObject"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    error_document = "MAINTENANCE.html"
    index_document = "MAINTENANCE.html"
  }
}

resource "aws_s3_bucket" "tfer--production-002D-palavyr-002D-userdata" {
  arn           = "arn:aws:s3:::production-palavyr-userdata"
  bucket        = "production-palavyr-userdata"
  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"
  request_payer       = "BucketOwner"

  tags = {
    dev-palavyr-previews = "dev"
  }

  tags_all = {
    dev-palavyr-previews = "dev"
  }

  versioning {
    enabled    = "true"
    mfa_delete = "false"
  }
}

resource "aws_s3_bucket" "tfer--staging-002D-palavyr-002D-userdata" {
  arn           = "arn:aws:s3:::staging-palavyr-userdata"
  bucket        = "staging-palavyr-userdata"
  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"
  request_payer       = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }
}

resource "aws_s3_bucket" "tfer--staging-002E-palavyr-002E-com" {
  arn    = "arn:aws:s3:::staging.palavyr.com"
  bucket = "staging.palavyr.com"

  cors_rule {
    allowed_headers = ["*"]
    allowed_methods = ["GET", "POST", "PUT"]
    allowed_origins = ["*"]
    max_age_seconds = "25000"
  }

  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Resource": "arn:aws:s3:::staging.palavyr.com/*",
      "Sid": "PublicReadGetObject"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    error_document = "index.html"
    index_document = "index.html"
  }
}

resource "aws_s3_bucket" "tfer--staging-002E-widget-002E-palavyr-002E-com" {
  arn    = "arn:aws:s3:::staging.widget.palavyr.com"
  bucket = "staging.widget.palavyr.com"

  cors_rule {
    allowed_headers = ["*"]
    allowed_methods = ["GET", "POST", "PUT"]
    allowed_origins = ["*"]
    max_age_seconds = "25000"
  }

  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Resource": "arn:aws:s3:::staging.widget.palavyr.com/*",
      "Sid": "PublicReadGetObject"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  tags = {
    palavyr = "widget"
  }

  tags_all = {
    palavyr = "widget"
  }

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    error_document = "index.html"
    index_document = "index.html"
  }
}

resource "aws_s3_bucket" "tfer--widget-002E-palavyr-002E-com" {
  arn    = "arn:aws:s3:::widget.palavyr.com"
  bucket = "widget.palavyr.com"

  cors_rule {
    allowed_headers = ["*"]
    allowed_methods = ["GET", "POST", "PUT"]
    allowed_origins = ["*"]
    max_age_seconds = "25000"
  }

  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"

  policy = <<POLICY
{
  "Statement": [
    {
      "Action": "s3:GetObject",
      "Effect": "Allow",
      "Principal": "*",
      "Resource": "arn:aws:s3:::widget.palavyr.com/*",
      "Sid": "PublicReadGetObject"
    }
  ],
  "Version": "2012-10-17"
}
POLICY

  request_payer = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    error_document = "index.html"
    index_document = "index.html"
  }
}

resource "aws_s3_bucket" "tfer--www-002E-palavyr-002E-com" {
  arn           = "arn:aws:s3:::www.palavyr.com"
  bucket        = "www.palavyr.com"
  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"
  request_payer       = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }
}

resource "aws_s3_bucket" "tfer--www-002E-staging-002E-palavyr-002E-com" {
  arn           = "arn:aws:s3:::www.staging.palavyr.com"
  bucket        = "www.staging.palavyr.com"
  force_destroy = "false"

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"
  request_payer       = "BucketOwner"

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }

  website {
    redirect_all_requests_to = "https://staging.palavyr.com"
  }
}

resource "aws_security_group" "tfer--Palavyr-002D-Security_sg-002D-0b658d1cd68796271" {
  description = "Allows Palavyr To do things"

  egress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = "0"
    protocol    = "-1"
    self        = "false"
    to_port     = "0"
  }

  ingress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = "0"
    protocol    = "-1"
    self        = "true"
    to_port     = "0"
  }

  name   = "Palavyr-Security"
  vpc_id = "vpc-0658b6edff9ad6795"
}

resource "aws_security_group" "tfer--default_sg-002D-02f79f93b33f82eb0" {
  description = "default VPC security group"

  egress {
    cidr_blocks = ["0.0.0.0/0"]
    from_port   = "0"
    protocol    = "-1"
    self        = "false"
    to_port     = "0"
  }

  ingress {
    from_port       = "0"
    prefix_list_ids = ["pl-02cd2c6b", "pl-63a5400a"]
    protocol        = "-1"
    security_groups = ["${data.terraform_remote_state.local.outputs.aws_security_group_tfer--Palavyr-002D-Security_sg-002D-0b658d1cd68796271_id}"]
    self            = "true"
    to_port         = "0"
  }

  name   = "default"
  vpc_id = "vpc-0658b6edff9ad6795"
}

resource "aws_subnet" "tfer--subnet-002D-03e90beae4cbc97e3" {
  assign_ipv6_address_on_creation                = "false"
  cidr_block                                     = "10.2.0.0/16"
  enable_dns64                                   = "false"
  enable_resource_name_dns_a_record_on_launch    = "false"
  enable_resource_name_dns_aaaa_record_on_launch = "false"
  ipv6_native                                    = "false"
  map_public_ip_on_launch                        = "false"
  private_dns_hostname_type_on_launch            = "ip-name"

  tags = {
    Name = "palavyr-subnet-3-public"
  }

  tags_all = {
    Name = "palavyr-subnet-3-public"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_subnet" "tfer--subnet-002D-0704cbef441993b25" {
  assign_ipv6_address_on_creation                = "false"
  cidr_block                                     = "10.1.0.0/16"
  enable_dns64                                   = "false"
  enable_resource_name_dns_a_record_on_launch    = "false"
  enable_resource_name_dns_aaaa_record_on_launch = "false"
  ipv6_native                                    = "false"
  map_public_ip_on_launch                        = "true"
  private_dns_hostname_type_on_launch            = "ip-name"

  tags = {
    Name = "palavyr-subnet-2"
  }

  tags_all = {
    Name = "palavyr-subnet-2"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_subnet" "tfer--subnet-002D-0ccabdc7222055adf" {
  assign_ipv6_address_on_creation                = "false"
  cidr_block                                     = "10.0.0.0/16"
  enable_dns64                                   = "false"
  enable_resource_name_dns_a_record_on_launch    = "false"
  enable_resource_name_dns_aaaa_record_on_launch = "false"
  ipv6_native                                    = "false"
  map_public_ip_on_launch                        = "true"
  private_dns_hostname_type_on_launch            = "ip-name"

  tags = {
    Name = "palavyr-subnet-1"
  }

  tags_all = {
    Name = "palavyr-subnet-1"
  }

  vpc_id = data.terraform_remote_state.local.outputs.aws_vpc_tfer--vpc-002D-0658b6edff9ad6795_id
}

resource "aws_vpc" "tfer--vpc-002D-0658b6edff9ad6795" {
  assign_generated_ipv6_cidr_block = "false"
  cidr_block                       = "10.0.0.0/16"
  enable_classiclink               = "false"
  enable_classiclink_dns_support   = "false"
  enable_dns_hostnames             = "true"
  enable_dns_support               = "true"
  instance_tenancy                 = "default"


  tags = {
    Name = "palavyr-vpc"
  }

  tags_all = {
    Name = "palavyr-vpc"
  }
}
