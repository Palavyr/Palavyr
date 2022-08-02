resource "aws_s3_bucket" "tfer--production-002D-palavyr-002D-userdata" {
  bucket        = var.bucket_name
  force_destroy = false

  grant {
    id          = "1db2ccca6338f55bdbf819083cdc61ff572716bf2cbbcc6299f32a0dfd3faed9"
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
    enabled    = "true"
    mfa_delete = "false"
  }
}
