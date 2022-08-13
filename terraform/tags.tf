locals {
  tags = {
    "Project"   = "palavyr-${lower(var.environment)}-${random_id.rand.hex}"
    "ManagedBy" = "terraform"
  }

  autoscale_tags = {
    "Project"   = "palavyr-${lower(var.environment)}-${random_id.rand.hex}"
    "ManagedBy" = "terraform"
    "name"      = "palavyr-autoscale"
  }
}
