
# This is the aws group of the machine users
# Its the main starting point for the palavyr setup right now
resource "aws_iam_group" "machine_users_group" {
  name = "machine_users-${lower(var.environment)}"
  path = "/machine_users"
}
