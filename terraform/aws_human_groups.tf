
# This is the aws group of my human users
# Its the main starting point for the palavyr setup right now
resource "aws_iam_group" "human_users_group" {
  name = "human_users-${lower(var.environment)}"
  path = "/human_users"
}
