
# This is the aws group of my human users
# Its the main starting point for the palavyr setup right now
resource "aws_iam_group" "human_users_group" {
  name = "human-users-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  path = "/humanusers/"
}
