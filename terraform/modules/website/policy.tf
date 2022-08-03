# data source to generate bucket policy to let OAI get objects:
data "aws_iam_policy_document" "bucket_policy_document" {
  statement {
    actions = ["s3:GetObject"]

    resources = [
      aws_s3_bucket.bucket.arn,
      "${aws_s3_bucket.bucket.arn}/*"
    ]

    principals {
      type        = "AWS"
      identifiers = [var.cloudfront_access_id_iam_arn]
    }
  }
}
