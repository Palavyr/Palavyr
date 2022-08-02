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
