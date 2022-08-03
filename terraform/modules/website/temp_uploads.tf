#upload website files to s3:
resource "aws_s3_object" "object" {
  bucket = aws_s3_bucket.bucket.id

  for_each     = fileset("uploads/", "*")
  key          = "${each.value}"
  source       = "uploads/${each.value}"
  etag         = filemd5("uploads/${each.value}")
  content_type = "text/html"

  depends_on = [
    aws_s3_bucket.bucket
  ]
}
