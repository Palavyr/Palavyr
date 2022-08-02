resource "aws_s3_bucket" "data" {
  bucket        = var.bucket_name
  force_destroy = false

  grant {
    permissions = ["FULL_CONTROL"]
    type        = "CanonicalUser"
  }

  hosted_zone_id      = "Z3AQBSTGFYJSTF"
  object_lock_enabled = "false"
  request_payer       = "BucketOwner"

  tags = {
    "Project"   = "Palavyr-${var.environment}"
    "ManagedBy" = "Terraform"
  }

  versioning {
    enabled    = "false"
    mfa_delete = "false"
  }
}
