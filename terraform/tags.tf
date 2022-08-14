locals {
  tags = {
    "Project"   = "palavyr-${lower(var.environment)}-${random_id.rand.hex}"
    "ManagedBy" = "terraform"
  }
}
