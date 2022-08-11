
# This is the aws group of the machine users
# Its the main starting point for the palavyr setup right now
resource "aws_iam_group" "machine_users_group" {
  name = "machine-users-${lower(var.environment)}-${lower(random_id.rand.hex)}"
  path = "/machineusers/"
}
